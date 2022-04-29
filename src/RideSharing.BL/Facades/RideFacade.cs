using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class RideFacade : CRUDFacade<RideEntity, RideRecentListModel, RideDetailModel>
{
    public RideFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) { }

    public override async Task<RideDetailModel?> GetAsync(Guid id)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();
        var ride = await dbSet
            .Include(ride => ride.Vehicle)
            //.ThenInclude(vehicle => vehicle.Owner)
            .SingleOrDefaultAsync(ride => ride.Id == id);
        return Mapper.Map<RideDetailModel>(ride);
    }

    public async Task<IEnumerable<RideFoundListModel>> GetFilteredAsync(Guid? userId, DateTime? dateFrom, DateTime? dateTo, string cityFrom, string cityTo)
    {
        if (userId == null)
        {
            return new List<RideFoundListModel>();
        }

        dateFrom ??= DateTime.Now;

        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();
        var rides = dbSet.Where(x =>
            x.Departure > dateFrom.Value &&
            (!dateTo.HasValue || x.Departure < dateTo.Value));

        // Not working probably due to EF bug
        //if (cityFrom != "")
        //{
        //    string[] filters = cityFrom.Split(new [] {' '});
        //    rides = rides.Where(x => filters.All(f => x.FromName.Contains(f)));
        //}
        //
        //if (cityTo != "")
        //{
        //    string[] filters = cityTo.Split(new[] { ' ' });
        //    rides = rides.Where(x => filters.All(f => x.ToName.Contains(f)));
        //}

        if (cityFrom != "")
        {
            rides = rides.Where(x =>  x.FromName.Equals(cityFrom));
        }
        if (cityTo != "")
        {
            rides = rides.Where(x => x.ToName.Equals(cityTo));
        }

        rides = rides.Include(ride => ride.Vehicle).ThenInclude(vehicle => vehicle!.Owner).Where(x => x.Vehicle!.OwnerId != userId);
        var rideModels = await Mapper.ProjectTo<RideFoundListModel>(rides).ToArrayAsync().ConfigureAwait(false);

        foreach (var ride in rideModels)
        {
            var reviews = uow.GetRepository<ReviewEntity>().Get().Where(x => x.RideId == ride.Id);
            ride.ReviewCount = await reviews.CountAsync();
            ride.Rating = await reviews.SumAsync(x => x.Rating) / (float) ride.ReviewCount;
            ride.OccupiedSeats = await uow.GetRepository<ReservationEntity>().Get().Where(x => x.RideId == ride.Id).SumAsync(x => x.Seats) + ride.Vehicle!.Seats - ride.SharedSeats;
        }
        return rideModels;
    }

    public async Task<IEnumerable<RideUpcomingListModel>> GetUserUpcomingRidesAsync(Guid? userId, bool driverFilter = false, bool passengerFilter = false)
    {
        if (userId == null)
        {
            return new List<RideUpcomingListModel>();
        }

        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();

        IQueryable<RideEntity> rides = driverFilter switch
        {
            true when !passengerFilter => dbSet.Where(x =>
                x.Departure > DateTime.Now && x.Vehicle != null && x.Vehicle.OwnerId == userId),
            false when passengerFilter => dbSet.Where(x =>
                x.Departure > DateTime.Now && x.Reservations.Any(y => y.ReservingUserId == userId)),
            _ => dbSet.Where(x => x.Departure > DateTime.Now && (x.Reservations.Any(y => y.ReservingUserId == userId) ||
                                                                 x.Vehicle != null && x.Vehicle.OwnerId == userId))
        };

        var rideModels = await Mapper.ProjectTo<RideUpcomingListModel>(rides.Include(x=>x.Vehicle)).ToArrayAsync().ConfigureAwait(false);

        foreach (var ride in rideModels)
        {
            ride.OccupiedSeats = await uow.GetRepository<ReservationEntity>().Get().Where(x => x.RideId == ride.Id).SumAsync(x => x.Seats) + ride.Vehicle.Seats - ride.SharedSeats;
            ride.IsDriver = await dbSet.Include(x => x.Vehicle).AnyAsync(x => x.Id == ride.Id && x.Vehicle!.OwnerId == userId);
        }
        return rideModels;
    }

    public async Task<IEnumerable<RideRecentListModel>> GetUserRecentRidesAsync(Guid? userId, bool driverFilter = false, bool passengerFilter = false)
    {
        if (userId == null)
        {
            return new List<RideRecentListModel>();
        }

        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();

        IQueryable<RideEntity> rides = driverFilter switch
        {
            true when !passengerFilter => dbSet.Where(x =>
                x.Departure <= DateTime.Now && x.Vehicle != null && x.Vehicle.OwnerId == userId),
            false when passengerFilter => dbSet.Where(x =>
                x.Departure <= DateTime.Now && x.Reservations.Any(y => y.ReservingUserId == userId)),
            _ => dbSet.Where(x =>
                x.Departure <= DateTime.Now && (x.Reservations.Any(y => y.ReservingUserId == userId) ||
                                                x.Vehicle != null && x.Vehicle.OwnerId == userId))
        };
        var rideModels = await Mapper.ProjectTo<RideRecentListModel>(rides).ToArrayAsync().ConfigureAwait(false);

        foreach (var ride in rideModels)
        {
            ride.IsDriver = await dbSet.Include(x => x.Vehicle).AnyAsync(x => x.Id == ride.Id && x.Vehicle!.OwnerId == userId);
            ride.HasReviewed = await uow.GetRepository<ReviewEntity>().Get().AnyAsync(x => x.RideId == ride.Id && x.AuthorUserId == userId);
        }
        return rideModels;
    }
}
