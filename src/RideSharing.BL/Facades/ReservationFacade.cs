using AutoMapper;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class ReservationFacade : CRUDFacade<ReservationEntity, ReservationListModel, ReservationDetailModel>
{
    public ReservationFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) { }
}
