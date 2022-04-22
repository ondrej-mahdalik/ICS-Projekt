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

    public async Task<IEnumerable<RideListModel>> GetFilteredAsync(DateTime? dateFrom = null, DateTime? dateTo = null, string? cityFrom = null, string? cityTo = null, int? seats = null)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();
        var rides = dbSet.Where(x =>
            (!dateFrom.HasValue || x.Departure > dateFrom.Value) &&
            (!dateTo.HasValue || x.Departure < dateTo.Value) &&
            (!seats.HasValue || x.SharedSeats >= seats.Value)
        );

        if (cityFrom != null)
        {
            string[] filters = cityFrom.Split(new [] {' '});
            rides = rides.Where(x => filters.All(f => x.FromName.Contains(f)));
        }

        if (cityTo != null)
        {
            string[] filters = cityTo.Split(new[] {' '});
            rides = rides.Where(x => filters.All(f => x.ToName.Contains((f))));
        }

        return await Mapper.ProjectTo<RideListModel>(rides).ToArrayAsync().ConfigureAwait(false);
    }
}
