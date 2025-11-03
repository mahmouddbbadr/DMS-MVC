using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.TrashViews;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Document = DMS.Domain.Models.Document;

namespace DMS.Service.Service
{
    public class TrashService : ITrashService
    {
        private readonly UnitOfWork _unit;

        public TrashService(UnitOfWork unit)
        {
            this._unit = unit;
        }
        public async Task<TrashViewModel> GetTrashOverviewAsync
            (TrashFilterViewModel filterModel)
        {
            return new TrashViewModel()
            {
                TrashFolders = await GetTrashedFolderAsync
                    (filterModel),

                TrashDocuments = await GetTrashedDocumentAsync
                    (filterModel)
            };
        }
        public async Task<TrashFoldersViewModel> GetTrashedFolderAsync(TrashFilterViewModel filterModel)
        {
            filterModel.SearchTerm = filterModel.SearchTerm?.Trim() ?? "";

            IQueryable<Folder> query = string.IsNullOrWhiteSpace(filterModel.SearchTerm)
                ? _unit.FolderRepository.GetDeletedFolders(filterModel.UserId)
                : _unit.FolderRepository.TrashedSearchAsQueryable(filterModel.SearchTerm, filterModel.UserId);

            int count = await query.CountAsync();

            var projectedQuery = query.Select(f => new TrashedFolderViewModel
            {
                Id = f.Id,
                Name = f.Name,
                DeletedAt = f.DeletedAt,
                ItemCount = f.Documents.Count()
            });

            /*
            projectedQuery = (filterModel.SortField, filterModel.SortOrder?.ToLower()) switch
            {
                ("Name", "asc") => projectedQuery.OrderBy(f => f.Name),
                ("Name", "desc") => projectedQuery.OrderByDescending(f => f.Name),

                ("ItemCount", "asc") => projectedQuery.OrderBy(f => f.ItemCount),
                ("ItemCount", "desc") => projectedQuery.OrderByDescending(f => f.ItemCount),

                ("DeletedAt", "asc") => projectedQuery.OrderBy(f => f.DeletedAt),
                ("DeletedAt", "desc") => projectedQuery.OrderByDescending(f => f.DeletedAt),

                _ => projectedQuery.OrderByDescending(f => f.DeletedAt)
            };
            */

            projectedQuery = SortFolderData(projectedQuery, filterModel.SortField!, filterModel.SortOrder!);

            var pagedItems = await projectedQuery
                .Skip((filterModel.PageNum - 1) * filterModel.PageSize)
                .Take(filterModel.PageSize)
                .ToListAsync();

            int totalPages = (int)Math.Ceiling(count / (double)filterModel.PageSize);

            return new TrashFoldersViewModel
            {
                TrashedFolders = new PagedResult<TrashedFolderViewModel>
                {
                    CurrentPage = filterModel.PageNum,
                    CurrentSearch = filterModel.SearchTerm,
                    TotalPages = totalPages,
                    SortField = filterModel.SortField,
                    SortOrder = filterModel.SortOrder,
                    Items = pagedItems
                }
            };
        }
        private IQueryable<TrashedFolderViewModel> SortFolderData
            (IQueryable<TrashedFolderViewModel> projectedQuery, string sortField, string sortOrder)
        {
            return (sortField, sortOrder?.ToLower()) switch
            {
                ("Name", "asc") => projectedQuery.OrderBy(f => f.Name),
                ("Name", "desc") => projectedQuery.OrderByDescending(f => f.Name),

                ("ItemCount", "asc") => projectedQuery.OrderBy(f => f.ItemCount),
                ("ItemCount", "desc") => projectedQuery.OrderByDescending(f => f.ItemCount),

                ("DeletedAt", "asc") => projectedQuery.OrderBy(f => f.DeletedAt),
                ("DeletedAt", "desc") => projectedQuery.OrderByDescending(f => f.DeletedAt),

                _ => projectedQuery.OrderByDescending(f => f.DeletedAt)
            };
        }

        public async Task<TrashDocumentsViewModel>GetTrashedDocumentAsync
            (TrashFilterViewModel filterModel)
        {
            filterModel.SearchTerm = filterModel.SearchTerm?.Trim() ?? "";

            IQueryable<Document> query;
            if(string.IsNullOrWhiteSpace(filterModel.SearchTerm))
            {
                query = _unit.DocumentRepository.GetDeleted(filterModel.UserId);
            }else
            {
                query = _unit.DocumentRepository.TrashedSearchAsQueryable(filterModel.SearchTerm, filterModel.UserId);
            }

            int count = query.Count();
            
            var projectedQuery = query.Select(d => new TrashedDocumentViewModel
            {
                Id = d.Id,
                DeletedAt = d.DeletedAt,
                FileType = d.FileType,
                FilePath = d.FilePath,
                FolderId = d.FolderId,
                Name = d.Name,
                Size = d.Size,
                FolderName = d.Folder.Name,
                FolderIsDeleted = d.Folder.IsDeleted
            });

            /*
            projectedQuery = (filterModel.SortField, filterModel.SortOrder?.ToLower()) switch
            {
                ("Name", "asc") => projectedQuery.OrderBy(d => d.Name),
                ("Name", "desc") => projectedQuery.OrderByDescending(d => d.Name),

                ("Size", "asc") => projectedQuery.OrderBy(d => d.Size),
                ("Size", "desc") => projectedQuery.OrderByDescending(d => d.Size),

                ("DeletedAt", "asc") => projectedQuery.OrderBy(d => d.DeletedAt),
                ("DeletedAt", "desc") => projectedQuery.OrderByDescending(d => d.DeletedAt),

                ("FolderName", "asc") => projectedQuery.OrderBy(d => d.FolderName),
                ("FolderName", "desc") => projectedQuery.OrderByDescending(d => d.FolderName),

                _ => projectedQuery.OrderByDescending(d => d.DeletedAt)
            };
            */
            
            projectedQuery = SortDocumentData(projectedQuery, filterModel.SortField!, filterModel.SortOrder!);

            List<Document> documents = await _unit.DocumentRepository.
                GetAllWithPaginationAsync(query, filterModel.PageNum, filterModel.PageSize);
            

            int totalPages = (int)Math.Ceiling((double) count / filterModel.PageSize);
            var trashDocumentsViewModel = new TrashDocumentsViewModel()
            {
                TrashedDocuments = new PagedResult<TrashedDocumentViewModel>()
                {
                    CurrentPage = filterModel.PageNum,
                    CurrentSearch = filterModel.SearchTerm,
                    TotalPages = totalPages,
                    SortField = filterModel.SortField,
                    SortOrder = filterModel.SortOrder,
                    Items = projectedQuery.ToList()
                }
            };
            return trashDocumentsViewModel;
        }

        private IQueryable<TrashedDocumentViewModel> SortDocumentData
            (IQueryable<TrashedDocumentViewModel> projectedQuery, string sortField, string sortOrder)
        {
            return (sortField, sortOrder?.ToLower()) switch
            {
                ("Name", "asc") => projectedQuery.OrderBy(d => d.Name),
                ("Name", "desc") => projectedQuery.OrderByDescending(d => d.Name),

                ("Size", "asc") => projectedQuery.OrderBy(d => d.Size),
                ("Size", "desc") => projectedQuery.OrderByDescending(d => d.Size),

                ("DeletedAt", "asc") => projectedQuery.OrderBy(d => d.DeletedAt),
                ("DeletedAt", "desc") => projectedQuery.OrderByDescending(d => d.DeletedAt),

                ("FolderName", "asc") => projectedQuery.OrderBy(d => d.FolderName),
                ("FolderName", "desc") => projectedQuery.OrderByDescending(d => d.FolderName),

                _ => projectedQuery.OrderByDescending(d => d.DeletedAt)
            };
        }
        public async Task<Folder?> GetFolderByIdAsync(string id, string userId)
        {
            Folder? folder = await _unit.FolderRepository
                .GetDeletedFolderByIdAsync(id, userId);
            return folder;
        }
        public async Task<Document?> GetDocumentByIdAsync(string id, string userId)
        {
            Document? document = await _unit.DocumentRepository
                .GetDeletedById(id, userId);
            return document;
        }

        public async Task<(bool Success, string Message)> RestoreFolderAsync(string folderId, string userId)
        {
            var folder = await GetFolderByIdAsync(folderId, userId);

            if (folder == null)
                return (false, "Folder not found or access denied.");

            try
            {
                folder.IsDeleted = false;
                folder.DeletedAt = null;

                _unit.FolderRepository.Update(folder);

                if (folder.Documents != null)
                {
                    foreach (var doc in folder.Documents)
                    {
                        doc.IsDeleted = false;
                        doc.DeletedAt = null;
                        _unit.DocumentRepository.Update(doc);
                    }
                }

                _unit.Save();
                return (true, "Folder restored successfully.");
            }
            catch
            {
                return (false, "Restore Folder could not be completed. Please try again.");
            }
        }

        public async Task<(bool Success, string Message)> RestoreDocumentAsync(string documentId, string userId)
        {
            var doc = await _unit.DocumentRepository.GetDeletedById(documentId, userId);

            if (doc == null)
                return (false, "Document not found or access denied.");

            if (doc.Folder != null && doc.Folder.IsDeleted)
                return (false, $"Restore the folder \"{doc.Folder.Name}\" first.");

            try
            {
                doc.IsDeleted = false;
                doc.DeletedAt = null;

                _unit.DocumentRepository.Update(doc);
                _unit.Save();

                return (true, "Restored successfully.");
            }
            catch
            {
                return (false, "Restore failed. Try again later.");
            }
        }

        public async Task<bool> DeleteFolderAsync(string folderId, string userId)
        {
            Folder? folder = await GetFolderByIdAsync(folderId, userId);
            if (folder == null)
                return false;

            try
            {
                await _unit.FolderRepository.DeleteAsync(folder.Id);
                _unit.Save();
                return true;
            }
            catch
            {
                throw new Exception("Delete Folder cannot be applied! Try again");
            }
        }
        public async Task<bool> DeleteDocumentAsync(string documentId, string userId, string wwwroot)
        {
            Document? doc = await GetDocumentByIdAsync(documentId, userId);
            if (doc == null)
                return false;

            try
            {
                await _unit.DocumentRepository.DeleteAsync(doc.Id);

                string fullPath = Path.Combine(wwwroot, doc.FilePath).Replace("/", "\\");
                if (File.Exists(fullPath))
                    File.Delete(fullPath);

                _unit.Save();
                return true;
            }
            catch
            {
                throw new Exception("Delete Document cannot be applied! Try again.");
            }
        }
    }
}
