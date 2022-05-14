using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RideSharing.BL.Models;
using RideSharing.DAL.Entities;
using RideSharing.DAL.UnitOfWork;

namespace RideSharing.BL.Facades;

public class ReservationFacade : CRUDFacade<ReservationEntity, ReservationListModel, ReservationDetailModel>
{
    public ReservationFacade(IUnitOfWorkFactory unitOfWorkFactory, IMapper mapper) : base(unitOfWorkFactory, mapper) { }

    public async Task<List<ReservationDetailModel>> GetReservationsByRideAsync(Guid rideId)
    {
        var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<ReservationEntity>().Get()
            .Include(x => x.ReservingUser)
            .Where(x => x.RideId == rideId);

        return await Mapper.ProjectTo<ReservationDetailModel>(dbSet).ToListAsync().ConfigureAwait(false);
    }

    public async Task<ReservationDetailModel?> GetUserReservationByRideAsync(Guid userId, Guid rideId)
    {
        var uow = UnitOfWorkFactory.Create();
        var dbSet = uow.GetRepository<ReservationEntity>().Get();
        var reservation = await dbSet.FirstOrDefaultAsync(x => x.ReservingUserId == userId && x.RideId == rideId);
        return reservation is null ? null : Mapper.Map<ReservationDetailModel>(reservation);
    }

    public async Task<bool> HasConflictingRide(Guid userId, Guid rideId)
    {
        var uow = UnitOfWorkFactory.Create();
        var dbSetReservations = uow.GetRepository<ReservationEntity>().Get();
        var dbSetRides = uow.GetRepository<RideEntity>().Get().Include(x => x.Vehicle);

        // Check if the user is the driver
        var ride = await dbSetRides.SingleOrDefaultAsync(x => x.Id == rideId);
        if (ride is null)
            throw new ArgumentException("Invalid ride id");

        if (ride.Vehicle is not null && ride.Vehicle.OwnerId == userId)
            return true;

        // Check for conflicting rides
        bool conflictReservation = await dbSetReservations.AnyAsync(
            x => x.ReservingUserId == userId && x.Ride != null &&
                 (ride.Departure <= x.Ride.Arrival || ride.Arrival >= x.Ride.Departure)
        );

        bool conflictRide = await dbSetRides.AnyAsync(
            x => x.Vehicle != null && x.Vehicle.Owner != null && x.Vehicle.Owner.Id == userId &&
                 (ride.Departure <= x.Arrival || ride.Arrival >= x.Departure)
        );
        return conflictRide || conflictReservation;

    }
}
