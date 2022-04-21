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

    public async Task<List<RideListModel>> GetFilteredAsync(DateTime? dateFrom = null, DateTime? dateTo = null, string? cityFrom = null, string? cityTo = null, int? seats = null)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<RideEntity>().Get();
        return new List<RideListModel>(); // TODO
    }
}
