using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class VehicleFacade : CRUDFacade<VehicleEntity, VehicleListModel, VehicleDetailModel>
{
    public VehicleFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) { }

    public async Task<IEnumerable<VehicleListModel>> GetByOwnerAsync(Guid ownerId)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var query = uow
            .GetRepository<VehicleEntity>()
            .Get()
            .Where(e => e.OwnerId == ownerId);
        return await Mapper.ProjectTo<VehicleListModel>(query).ToArrayAsync().ConfigureAwait(false);
    }
}
