using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class VehicleFacade : CRUDFacade<VehicleEntity, VehicleListModel, VehicleDetailModel>
{
    public VehicleFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) { }

    public async Task<List<VehicleListModel>> GetByOwnerAsync(Guid ownerId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var query = uow
            .GetRepository<VehicleEntity>()
            .Get()
            .Where(e => e.OwnerId == ownerId);
        return await Mapper.ProjectTo<VehicleListModel>(query).ToListAsync().ConfigureAwait(false);
    }

    public override async Task DeleteAsync(Guid id)
    {
        // Delete related rides (and their reservations)
        await using var uow = UnitOfWorkFactory.Create();
        var rides = uow.GetRepository<RideEntity>().Get().Where(x => x.VehicleId == id);
        uow.GetRepository<RideEntity>().DeleteRange(rides.Select(x => x.Id));
        await uow.CommitAsync();

        await base.DeleteAsync(id);
    }
}
