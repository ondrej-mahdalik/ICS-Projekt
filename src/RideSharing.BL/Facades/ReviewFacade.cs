using AutoMapper;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class ReviewFacade : CRUDFacade<ReviewEntity, ModelBase, ReviewDetailModel> // Replaced ReviewListModel with ModelBase (might not work as intended)
{
    public ReviewFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) { }
}
