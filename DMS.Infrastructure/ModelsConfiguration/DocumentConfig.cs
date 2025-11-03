using DMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DMS.Infrastructure.ModelsConfiguration
{
    public class DocumentConfig : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            builder.HasIndex(d => new { d.OwnerId, d.FolderId, d.Name })
                .IsUnique();

            builder.HasQueryFilter(d => !d.IsDeleted);

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(d => d.FilePath)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(d => d.FileType)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(d => d.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(d => d.Size)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(d => d.AddedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.Property(d => d.DeletedAt)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.HasOne(d => d.Folder)
                .WithMany(f => f.Documents)
                .HasForeignKey(d => d.FolderId);
        }
    }
}
