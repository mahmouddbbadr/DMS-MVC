using DMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.Infrastructure.ModelsConfiguration
{
    public class SharedItemConfig : IEntityTypeConfiguration<SharedItem>
    {
        public void Configure(EntityTypeBuilder<SharedItem> builder)
        {
            builder.ToTable("SharedItem");

            builder.HasKey(s => s.Id);

            builder.Property(s => s.PermissionLevel)
                .HasMaxLength(20)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(d => d.AddedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(s => s.SharedWithUser)
               .WithMany(u => u.ReceivedSharedItems)
               .HasForeignKey(s => s.SharedWithUserId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.SharedByUser)
                   .WithMany(u => u.SharedItems)
                   .HasForeignKey(s => s.SharedByUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Document)
               .WithMany(u => u.SharedDocument)
               .HasForeignKey(s => s.DocumentId)
               .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Folder)
                   .WithMany(u => u.SharedFolders)
                   .HasForeignKey(s => s.FolderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
