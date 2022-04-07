using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class UserFacade : CRUDFacade<UserEntity, UserListModel, UserDetailModel>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWorkFactory _unitOfWorkFactory;

    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper)
    {
        _mapper = mapper;
        _unitOfWorkFactory = unitOfWorkFactory;
    }

    //public new async Task DeleteAsync(Guid id)
    //{
    //    // Delete all reviews the user has received
    //    var reviewFacade = new ReviewFacade(_unitOfWorkFactory, _mapper);
    //    var reviews = (await reviewFacade.GetAsync()).Where(x => x.ReviewedUser?.Id == id);
    //    foreach (var review in reviews)
    //        await reviewFacade.DeleteAsync(review);

    //    await using var uow = _unitOfWorkFactory.Create();


    //    // Delete the user
    //    uow.GetRepository<UserEntity>().Delete(id);

    //    // Commit changes
    //    await uow.CommitAsync();
    //}
}
