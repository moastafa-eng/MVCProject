using System.ComponentModel.DataAnnotations.Schema;

namespace GymManagementDAL.Entities
{
    [Table("Members")]
    public class HealthRecord : BaseEntity
    {
        public Decimal Weight { get; set; }
        public Decimal Height { get; set; }
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }

    }
}
