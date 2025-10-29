using DMS.Domain.ENums;
using DMS.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.Seed
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder model)
        {
            SeedUsers(model);
            SeedFolders(model);
            SeedDocuments(model);
            SeedRoles(model);
            SeedUserRoles(model);
            SeedSharedItems(model);
        }
        private static void SeedUsers(ModelBuilder modelBuilder)
        {
            var user = new AppUser
            {
                Id = "user-1",
                FName = "Ahmed",
                LName = "Fergany",
                PasswordHash = "AQAAAAIAAYagAAAAEGJrDwU8NV1Kx9oW8Gc+UgFTpj6qTr7mk0bh7eEXMHcbIuhcw/JW3v9s5pr7oNV8IQ==",
                IsLocked = false,
                Email = "ahmed@example.com",
                NormalizedEmail = "AHMED@EXAMPLE.COM",
                EmailConfirmed = true,
                UserName = "ahmed.fergany",
                NormalizedUserName = "AHMED.FERGANY",
                PhoneNumber = "01000000000",
                PhoneNumberConfirmed = true,
                SecurityStamp = "e3c1c6d2-9d48-4a90-a9a4-1234567890ab",
                ConcurrencyStamp = "b7c1a4d2-8b23-4b12-b5b4-abcdefabcdef",
                CreatedAt = new DateTime(2025, 10, 1, 12, 0, 0, DateTimeKind.Utc)
            };

            var user2 = new AppUser
            {
                Id = "user-2",
                FName = "Mahmoud",
                LName = "Badr",
                PasswordHash = "AQAAAAIAAYagAAAAEGJrDwU8NV1Kx9oW8Gc+UgFTpj6qTr7mk0bh7eEXMHcbIuhcw/JW3v9s5pr7oNV8IQ==",
                IsLocked = false,
                Email = "mahmoud@example.com",
                NormalizedEmail = "MAHMOUD@EXAMPLE.COM",
                EmailConfirmed = true,
                UserName = "mahmoud.badr",
                NormalizedUserName = "MAHMOUD.BADR",
                PhoneNumber = "01000000000",
                PhoneNumberConfirmed = true,
                SecurityStamp = "6d89f59e-bbfa-49d0-8e2e-a1b1e872e24d",
                ConcurrencyStamp = "f0d3e78b-4c8f-464e-b78f-e3b56c1487cc",
                CreatedAt = new DateTime(2025, 10, 2, 12, 0, 0, DateTimeKind.Utc)
            };


            var user3 = new AppUser
            {
                Id = "user-3",
                FName = "Abdo",
                LName = "Ahmed",
                PasswordHash = "AQAAAAIAAYagAAAAEGJrDwU8NV1Kx9oW8Gc+UgFTpj6qTr7mk0bh7eEXMHcbIuhcw/JW3v9s5pr7oNV8IQ==",
                IsLocked = false,
                Email = "abdo@example.com",
                NormalizedEmail = "ABDO@EXAMPLE.COM",
                EmailConfirmed = true,
                UserName = "abdo.ahmed",
                NormalizedUserName = "ABDO.AHMED",
                PhoneNumber = "01000000000",
                PhoneNumberConfirmed = true,
                SecurityStamp = "a6bdc263-9d47-4fd8-b929-101e1a99d9af",
                ConcurrencyStamp = "ab21854a-bbdf-4342-a86c-421c1d932f28",
                CreatedAt = new DateTime(2025, 10, 3, 12, 0, 0, DateTimeKind.Utc)
            };


            modelBuilder.Entity<AppUser>().HasData(user, user2, user3);
        }

        private static void SeedFolders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Folder>().HasData(
                new Folder
                {
                    Id = "folder-root",
                    Name = "Root Folder",
                    OwnerId = "user-2",
                    ParentFolderId = null, 
                    IsDeleted = false,
                    IsStarred = false,
                    AddedAt = new DateTime(2025, 10, 3, 12, 0, 0, DateTimeKind.Utc)
                },
                new Folder
                {
                    Id = "folder-child",
                    Name = "Child Folder",
                    OwnerId = "user-2",
                    ParentFolderId = "folder-root",
                    IsDeleted = false,
                    IsStarred = false,
                    AddedAt = new DateTime(2025, 10, 3, 12, 5, 0, DateTimeKind.Utc)
                }
            );
        }

        private static void SeedDocuments(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Document>().HasData(
                new Document
                {
                    Id = "doc-1",
                    Name = "SampleDoc",
                    FilePath = "files/sample.pdf",
                    FileType = "pdf",
                    Size = 1024,
                    FolderId = "folder-child",
                    OwnerId = "user-2",
                    IsDeleted = false,
                    IsStarred = false,
                    AddedAt = new DateTime(2025, 10, 3, 12, 50, 0, DateTimeKind.Utc)
                }
            );
        }

        private static void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "role-admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "role-user",
                    Name = "User",
                    NormalizedName = "USER"
                }
            );
        }

        private static void SeedUserRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "user-1",
                    RoleId = "role-admin"
                }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "user-2",
                    RoleId = "role-user"
                }
            );
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = "user-3",
                    RoleId = "role-user"
                }
            );
        }

        private static void SeedSharedItems(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SharedItem>().HasData(
                new SharedItem
                {
                    Id = "share-1",
                    PermissionLevel = PermissionLevel.Read,
                    SharedByUserId = "user-2",
                    SharedWithUserId = "user-3",
                    FolderId = "folder-child",
                    DocumentId = null,
                    AddedAt = new DateTime(2025, 10, 10, 12, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
