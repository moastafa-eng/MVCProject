using GymManagementDAL.Entities.Enums;

namespace GymManagementDAL.Entities
{
    public class Trainer : GymUser
    {
        public specialities specialities { get; set; }
        public ICollection<Session> Sessions { get; set; } = null!;
    }
}
