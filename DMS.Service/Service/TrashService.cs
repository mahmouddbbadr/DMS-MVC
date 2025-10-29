using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.TrashViews;
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
        public async Task<TrashFoldersViewModel>GetTrashedFolderAsync
            (TrashFilterViewModel filterModel)
        {
            filterModel.SearchTerm = filterModel.SearchTerm?.Trim() ?? "";

            IQueryable<Folder> query;
            if (string.IsNullOrWhiteSpace(filterModel.SearchTerm))
            {
                query = _unit.FolderRepository.GetDeleted(filterModel.UserId);
            }
            else
            {
                query = _unit.FolderRepository.TrashedSearchAsQueryable(filterModel.SearchTerm, filterModel.UserId);
            }

            int count = query.Count();

            query = (filterModel.SortField, filterModel.SortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(f => f.Name),
                ("Name", "desc") => query.OrderByDescending(f => f.Name),

                ("ItemCount", "asc") => query.OrderBy(f => f.Documents!.Count()),
                ("ItemCount", "desc") => query.OrderByDescending(f => f.Documents!.Count()),

                ("DeletedAt", "asc") => query.OrderBy(f => f.DeletedAt),
                ("DeletedAt", "desc") => query.OrderByDescending(f => f.DeletedAt),

                _ => query.OrderByDescending(f => f.DeletedAt)
            };

            List<Folder> folders = await _unit.FolderRepository.
                GetAllWithPaginationAsync(query, filterModel.PageNum, filterModel.PageSize);

            int totalPages = (int)Math.Ceiling((double)count / filterModel.PageSize);
            var trashFoldersViewModel = new TrashFoldersViewModel()
            {
                TrashedFolders = new PagedResult<TrashedFolderViewModel>()
                {
                    CurrentPage = filterModel.PageNum,
                    CurrentSearch = filterModel.SearchTerm,
                    TotalPages = totalPages,
                    SortField = filterModel.SortField,
                    SortOrder = filterModel.SortOrder,
                    Items = folders.Select(d => new TrashedFolderViewModel()
                    {
                        Id = d.Id,
                        DeletedAt = d.DeletedAt,
                        ItemCount = d.Documents?.Count(),
                        Name = d.Name,
                        ParentFolderName = d.ParentFolder?.Name
                    }).ToList()
                }
            };
            return trashFoldersViewModel;
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

            query = (filterModel.SortField, filterModel.SortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(d => d.Name),
                ("Name", "desc") => query.OrderByDescending(d => d.Name),

                ("Size", "asc") => query.OrderBy(d => d.Size),
                ("Size", "desc") => query.OrderByDescending(d => d.Size),

                ("DeletedAt", "asc") => query.OrderBy(d => d.DeletedAt),
                ("DeletedAt", "desc") => query.OrderByDescending(d => d.DeletedAt),

                ("FolderName", "asc") => query.OrderBy(d => d.Folder!.Name),
                ("FolderName", "desc") => query.OrderByDescending(d => d.Folder!.Name),

                _ => query.OrderByDescending(d => d.DeletedAt)
            };

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
                    Items = documents.Select(d => new TrashedDocumentViewModel()
                    {
                        Id = d.Id,
                        DeletedAt = d.DeletedAt,
                        FileType = d.FileType,
                        FilePath = d.FilePath,
                        FolderId = d.FolderId,
                        Name = d.Name,
                        Size = d.Size,
                        FolderName = d.Folder?.Name                      
                    }).ToList()
                }
            };
            return trashDocumentsViewModel;
        }

        public async Task<Folder?> GetFolderByIdAsync(string id, string userId)
        {
            Folder? folder = await _unit.FolderRepository.GetDeletedById(id, userId);
            return folder;
        }
        public async Task<Document?> GetDocumentByIdAsync(string id, string userId)
        {
            Document? document = await _unit.DocumentRepository.GetDeletedById(id, userId);
            return document;
        }

        public async Task<bool> RestoreFolderAsync(Folder folder)
        {
           try
           {
                folder.IsDeleted = false;
                folder.DeletedAt = null;
                _unit.Save();
                return true;
           }catch (Exception ex)
           {
                throw new Exception("Restore Folder can not applied! Try again", ex);
           }
        }
        public async Task<bool> RestoreDocumentAsync(Document document)
        {
            try
            {
                document.IsDeleted = false;
                document.DeletedAt = null;
                _unit.Save();
                return true;
            }catch (Exception ex)
            {
                throw new Exception("Restore Document can not applied! Try again", ex);
            }
        }
        public async Task<bool> DeleteFolderAsync(string folderId, string userId)
        {
            Folder? folder = await GetFolderByIdAsync(folderId, userId);
            if (folder == null)
                throw new Exception("Folder not found. Or can not access");
            try
            {
                await _unit.FolderRepository.DeleteAsync(folderId);
                _unit.Save();
                return true;
            }catch (Exception ex)
            {
                throw new Exception("Delete Folder can not applied! Try again", ex);
            }
        }
        public async Task<bool> DeleteDocumentAsync(string documentId, string userId, string wwwroot)
        {
            Document? doc = await GetDocumentByIdAsync(documentId, userId);
            if(doc == null)
                throw new Exception("Folder not found. Or can not access");

            try
            {
                await _unit.DocumentRepository.DeleteAsync(documentId);

                string deletePath = Path.Combine(wwwroot, doc.FilePath).Replace("/", "\\");
                if (File.Exists(deletePath))
                    File.Delete(deletePath);

                _unit.Save();
                return true;
            }catch (Exception ex)
            {
                throw new Exception("Delete Document can not applied! Try again", ex);
            }
        }
    }
}
