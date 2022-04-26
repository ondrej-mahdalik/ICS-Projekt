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

    public async Task<IEnumerable<UserListModel>> GetUsers()
    {
        await using var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<UserEntity>().Get();

        var users =  await Mapper.ProjectTo<UserListModel>(dbSet).ToArrayAsync().ConfigureAwait(false);

        foreach (var user in users)
        {
            user.NumberOfVehicles = uow.GetRepository<VehicleEntity>().Get().Where(x => x.OwnerId == user.Id).Count();
            user.UpcomingRidesCount = uow.GetRepository<RideEntity>().Get().Where(x => x.Departure > DateTime.Now &&
                (x.Reservations.Any(y => y.ReservingUserId == user.Id) ||
                x.Vehicle != null && x.Vehicle.OwnerId == user.Id )).Count();
        }
        return users;
    }
}
