namespace RideSharing.DAL.Entity
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        ICollection<Review> Reviews { get; set; }
        ICollection<Vehicle> Vehicles { get; set; }
    }
}
