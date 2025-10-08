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
    public class SessionConfiguration : IEntityTypeConfiguration<Session>
    {
        public void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.HasOne(t => t.Trainer)
                .WithMany(t => t.Sessions)
                .HasForeignKey(t => t.TrainerId);

            builder.HasOne(c => c.Category)
                .WithMany(c => c.Sessions)
                .HasForeignKey(c => c.CategoryId);

            builder.ToTable( x => 
            {
                x.HasCheckConstraint("Session_CapacityCheck", "Capacity between 1 and 25 ");
                x.HasCheckConstraint("Session_CeckEndDate", "EndDate > StartDate");

            });
        }
    }
}
