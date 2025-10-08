using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    public class MemberShipConfiguration : IEntityTypeConfiguration<MemberShip>
    {
        public void Configure(EntityTypeBuilder<MemberShip> builder)
        {
            builder.Ignore(i => i.Id);

            builder.Property(c => c.CreatedAt)
                .HasColumnName("StartDate")
                .HasDefaultValue("GETDATE()");

            builder.HasOne(m => m.Member)
                .WithMany(m => m.MemberPlans)
                .HasForeignKey(m => m.MemberId);

            builder.HasOne(p => p.Plan)
                .WithMany(p => p.PlanMembers)
                .HasForeignKey(p => p.PlanId);

            builder.HasKey(i => new { i.MemberId, i.PlanId });
        }
    }
}
