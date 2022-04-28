using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class RideFacade : CRUDFacade<RideEntity, RideListModel, RideDetailModel>
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

    public async Task<IEnumerable<FoundRideModel>> GetFilteredAsync(DateTime? dateFrom, DateTime? dateTo, string cityFrom, string cityTo, int seats)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();
        var rides = dbSet.Where(x =>
            (!dateFrom.HasValue || x.Departure > dateFrom.Value) &&
            (!dateTo.HasValue || x.Departure < dateTo.Value) &&
            (x.SharedSeats >= seats)
        );

        //if (cityFrom != "")
        //{
        //    string[] filters = cityFrom.Split(new [] {' '});
        //    rides = rides.Where(x => filters.All(f => x.FromName.Contains(f)));
        //}

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

        rides = rides.Include(ride => ride.Vehicle)
                     .ThenInclude(vehicle => vehicle.Owner);
        var foundRideModels = await Mapper.ProjectTo<FoundRideModel>(rides).ToArrayAsync().ConfigureAwait(false);
        foreach (var ride in foundRideModels)
        {
            var reviews = uow.GetRepository<ReviewEntity>().Get().Where(x => x.RideId == ride.Id);
            int reviewCount = await reviews.CountAsync();
            int ratingSum = 0;
            //foreach (var review in reviews)
            //{
            //   ratingSum += review.Rating;
            //}
            float rating = await reviews.SumAsync(x => x.Rating) / (float) reviewCount;
            ride.ReviewCount = reviewCount;
            ride.Rating = rating;
        }

        return foundRideModels;
    }

    public async Task<IEnumerable<RideListModel>> GetUserUpcomingRidesAsync(Guid userId, bool driverFilter = false, bool passengerFilter = false)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();

        IQueryable<RideEntity> rides;

        if (driverFilter && !passengerFilter)
            rides = dbSet.Where(x => x.Departure > DateTime.Now && x.Vehicle != null && x.Vehicle.OwnerId == userId);
        else if (!driverFilter && passengerFilter)
            rides = dbSet.Where(x => x.Departure > DateTime.Now && x.Reservations.Any(y => y.ReservingUserId == userId));
        else
            rides = dbSet.Where(x => x.Departure > DateTime.Now && (x.Reservations.Any(y => y.ReservingUserId == userId) || x.Vehicle != null && x.Vehicle.OwnerId == userId));

        return await Mapper.ProjectTo<RideListModel>(rides).ToArrayAsync().ConfigureAwait(false);
    }

    public async Task<IEnumerable<RideListModel>> GetUserRecentRidesAsync(Guid userId, bool driverFilter = false, bool passengerFilter = false)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();

        IQueryable<RideEntity> rides;

        if (driverFilter && !passengerFilter)
        {
            rides = dbSet.Where(x => x.Departure <= DateTime.Now && x.Vehicle != null && x.Vehicle.OwnerId == userId);
        }
        else if (!driverFilter && passengerFilter)
        {
            rides = dbSet.Where(x => x.Departure <= DateTime.Now && x.Reservations.Any(y => y.ReservingUserId == userId));
        }
        else
        {
            rides = dbSet.Where(x => x.Departure <= DateTime.Now && (x.Reservations.Any(y => y.ReservingUserId == userId) || x.Vehicle != null && x.Vehicle.OwnerId == userId));
        }

        return await Mapper.ProjectTo<RideListModel>(rides).ToArrayAsync().ConfigureAwait(false);
    }
}
