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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.Ignore(i => i.Id);

            builder.Property(c => c.CreatedAt)
                .HasColumnName("BookingDate")
                .HasDefaultValue("GETDATE()");

            builder.HasOne(s => s.Session)
                .WithMany(s => s.SessionMembers)
                .HasForeignKey(s => s.SessionId);

            builder.HasOne(m => m.Member)
                .WithMany(m => m.MemberSessions)
                .HasForeignKey(m => m.MemberId);

            builder.HasKey(i => new { i.MemberId, i.SessionId });
        }
    }
}
