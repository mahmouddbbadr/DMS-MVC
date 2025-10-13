using DMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.Infrastructure.ModelsConfiguration
{
    public class FolderConfig : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.ToTable("Folders");

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(f => f.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(f => f.CreatedAt)
              .HasColumnType("datetime2")
              .HasDefaultValueSql("GETDATE()");

            builder.HasOne(f => f.ParentFolder)
                .WithMany(f => f.Folders)
                .HasForeignKey(f => f.FolderId);
        }
    }
}
