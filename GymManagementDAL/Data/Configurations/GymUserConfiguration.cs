using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementDAL.Data.Configurations
{
    public class GymUserConfiguration : IEntityTypeConfiguration<GymUser>
    {
        public void Configure(EntityTypeBuilder<GymUser> builder)
        {
            builder.Property(n => n.Name)
                .HasColumnType("varchar")
                .HasMaxLength(50);

            builder.Property(e => e.Email)
                .HasColumnType("varchar")
                .HasMaxLength(100);

            builder.Property(p => p.Phone)
                .HasColumnType("varchar")
                .HasMaxLength(11);

            builder.OwnsOne(d => d.Address, Address =>
            {
                Address.Property(b => b.BuildingNumber)
                .HasColumnName("BuildingNumber");

                Address.Property(s => s.Street)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .HasColumnName("Street");

                Address.Property(c => c.City)
                .HasColumnType("varchar")
                .HasMaxLength(30)
                .HasColumnName("City");

            });

            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(p => p.Phone).IsUnique();

            builder.ToTable(x =>
            {

            x.HasCheckConstraint("Gym_CheckEmail", "Email LIKE '_%@_%._%'");
            x.HasCheckConstraint("Gym_CheckPhone", "Phone LIKE '01%' and Phone Not Like '%[^0-9]%'");
            });

            
        }
    }
}
