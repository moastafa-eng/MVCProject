using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    public class MemberConfiguration : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(c => c.CreatedAt)
                .HasColumnName("JoinDate")
                .HasDefaultValue("GETDATE()");

            builder.HasOne(h => h.HealthRecord)
                .WithOne()
                .HasForeignKey<HealthRecord>(h => h.Id);
        }
    }
}
