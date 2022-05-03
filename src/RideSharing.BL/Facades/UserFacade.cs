using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
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
            user.NumberOfVehicles = await uow.GetRepository<VehicleEntity>().Get().CountAsync(x => x.OwnerId == user.Id);
            user.UpcomingRidesCount = await uow.GetRepository<RideEntity>().Get().CountAsync(x => x.Departure > DateTime.Now &&
                (x.Reservations.Any(y => y.ReservingUserId == user.Id) ||
                 x.Vehicle != null && x.Vehicle.OwnerId == user.Id));
        }
        return users;
    }

    public override async Task DeleteAsync(Guid id)
    {
        // Manually delete any rides where the user is a driver
        await using var uow = UnitOfWorkFactory.Create();
        var rides = uow.GetRepository<VehicleEntity>().Get().Where(x => x.OwnerId == id).SelectMany(x => x.Rides);
        uow.GetRepository<RideEntity>().DeleteRange(rides.Select(x => x.Id));
        await uow.CommitAsync().ConfigureAwait(false);

        await base.DeleteAsync(id);
    }


    public async Task<string> UploadImageAsync(string filePath)
    {
        // Open file
        var extension = Path.GetExtension(filePath);
        var fileStream = File.OpenRead(filePath);
        var contentType = extension switch
        {
            ".png" => "image/png",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".bmp" => "image/bmp",
            ".gif" => "image/gif",
            _ => throw new FormatException(),
        };

        // Upload content
        GoogleCredential credentials = GoogleCredential.FromJson(await File.ReadAllTextAsync(@"google-cloud-credentials.json"));
        var client = await StorageClient.CreateAsync(credentials);
        var result = await client.UploadObjectAsync("ics-ridesharing", $"ProfilePictures/{DateTime.Now:yyyyMMddHHmmssffff}{extension}", contentType,
            fileStream);

        return result.MediaLink;
    }
}
