using AutoMapper;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class VehicleFacade : CRUDFacade<VehicleEntity, VehicleListModel, VehicleDetailModel>
{
    public VehicleFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) { }
}
