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
            x => x.ReservingUserId == userId && x.Ride != null && (
                 (ride.Departure <= x.Ride.Arrival && x.Ride.Arrival <= ride.Arrival) ||
                 (ride.Departure <= x.Ride.Departure && x.Ride.Departure <= ride.Arrival) ||
                 (x.Ride.Departure <= ride.Arrival && ride.Arrival <= x.Ride.Arrival) ||
                 (x.Ride.Departure <= ride.Departure && ride.Departure <= x.Ride.Arrival))
        );

        bool conflictRide = await dbSetRides.AnyAsync(
            x => x.Vehicle != null && x.Vehicle.Owner != null && x.Vehicle.Owner.Id == userId && (
                 (ride.Departure <= x.Arrival && x.Arrival <= ride.Arrival) ||
                 (ride.Departure <= x.Departure && x.Departure <= ride.Arrival) ||
                 (x.Departure <= ride.Arrival && ride.Arrival <= x.Arrival) ||
                 (x.Departure <= ride.Departure && ride.Departure <= x.Arrival))
        );
        return conflictRide || conflictReservation;

    }

    public async Task<bool> HasConflictingRide(Guid userId, DateTime departure, DateTime arrival)
    {
        var uow = UnitOfWorkFactory.Create();
        var dbSetReservations = uow.GetRepository<ReservationEntity>().Get();
        var dbSetRides = uow.GetRepository<RideEntity>().Get();

        // Check for conflicting rides
        bool conflictReservation = await dbSetReservations.AnyAsync(
            x => x.ReservingUserId == userId && x.Ride != null && (
                (departure <= x.Ride.Arrival && x.Ride.Arrival <= arrival) ||
                (departure <= x.Ride.Departure && x.Ride.Departure <= arrival) ||
                (x.Ride.Departure <= arrival && arrival <= x.Ride.Arrival) ||
                (x.Ride.Departure <= departure && departure <= x.Ride.Arrival))
        );

        bool conflictRide = await dbSetRides.AnyAsync(
            x => x.Vehicle != null && x.Vehicle.Owner != null && x.Vehicle.Owner.Id == userId && (
                (departure <= x.Arrival && x.Arrival <= arrival) ||
                (departure <= x.Departure && x.Departure <= arrival) ||
                (x.Departure <= arrival && arrival <= x.Arrival) ||
                (x.Departure <= departure && departure <= x.Arrival))
        );
        return conflictRide || conflictReservation;

    }
}
