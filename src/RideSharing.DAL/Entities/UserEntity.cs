namespace RideSharing.DAL.Entities
{
    public record UserEntity(
        Guid Id, string Name, string Surname, string Phone) : IEntity
    {
        private ICollection<ReviewEntity> Reviews { get; set; } = new List<ReviewEntity>();
        private ICollection<VehicleEntity> Vehicles { get; set; } = new List<VehicleEntity>();
    }
}
