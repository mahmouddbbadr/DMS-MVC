using DMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.Infrastructure.ModelsConfiguration
{
    public class DocumentConfig : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable("Documents");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(d => d.FilePath)
                .HasColumnType("VARCHAR(255)")
                .IsRequired();

            builder.Property(d => d.FileType)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(d => d.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(d => d.Size)
                .HasColumnType("int")
                .IsRequired();

            builder.Property(d => d.UploadedAt)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(d => d.Folder)
                .WithMany(f => f.Documents)
                .HasForeignKey(d => d.FolderId);
        }
    }
}
