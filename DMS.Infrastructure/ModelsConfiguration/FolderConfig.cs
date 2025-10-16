﻿using DMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DMS.Infrastructure.ModelsConfiguration
{
    public class FolderConfig : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.ToTable("Folders");

            builder.HasQueryFilter(f => !f.IsDeleted);

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Name)
                .HasColumnType("VARCHAR(20)")
                .IsRequired();

            builder.Property(f => f.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(f => f.AddedAt)
              .HasColumnType("datetime2")
              .HasDefaultValueSql("GETDATE()");

            builder.HasOne(sf => sf.ParentFolder)
                .WithMany(pf => pf.SubFolders)
                .HasForeignKey(sf => sf.ParentFolderId);
        }
    }
}
