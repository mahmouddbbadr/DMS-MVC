using DMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.ModelsConfiguration
{
    public class AppUserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasIndex(u => u.WorkSpaceName)
                .IsUnique();

            builder.Property(u => u.FName)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(u => u.LName)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(u => u.Address)
                .HasColumnType("VARCHAR(20)")
                .IsRequired(false);

            builder.Property(u => u.IsLocked)
                .HasDefaultValue(false);

            builder.Property(u => u.WorkSpaceName)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.HasMany(u => u.Folders)
                .WithOne(f => f.AppUser)
                .HasForeignKey(f => f.AppUserId);

        }
    }
}
