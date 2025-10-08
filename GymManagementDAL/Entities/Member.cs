namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        public string? Image { get; set; }
        public HealthRecord HealthRecord { get; set; } = null!;
        public ICollection<MemberShip> MemberPlans { get; set; } = null!;
        public ICollection<Booking> MemberSessions { get; set; } = null!;

    }
}
