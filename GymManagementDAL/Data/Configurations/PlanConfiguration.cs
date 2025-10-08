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
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.Property(n => n.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(d => d.Description)
                .HasColumnType("varchar")
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(10, 2);

            builder.ToTable(D =>
            {
                D.HasCheckConstraint("Plan_CheckDuration", "DurationDays Between 1 and 365");
            });
        }
    }
}
