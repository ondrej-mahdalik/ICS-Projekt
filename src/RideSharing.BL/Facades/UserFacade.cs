using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class UserFacade : CRUDFacade<UserEntity, UserListModel, UserDetailModel>
{
    public UserFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) { }

    public override async Task<UserDetailModel?> GetAsync(Guid id)
    {
        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<UserEntity>().Get();
        var user = await dbSet
            .Include(user => user.SubmittedReviews)
            .Include(user => user.Vehicles)
            .Include(user => user.Reservations)
            //.ThenInclude(review => review.AuthorUser)
            .SingleOrDefaultAsync(user => user.Id == id);
        return Mapper.Map<UserDetailModel>(user);
    }
}
