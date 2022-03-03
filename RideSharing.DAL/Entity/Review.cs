namespace RideSharing.DAL.Entity
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public Guid RideId { get; set; }
        public Guid UserId { get; set; }
        public ushort Rating { get; set; }
    }
}
