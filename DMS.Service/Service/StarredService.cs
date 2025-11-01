using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.StarredViews;
using DMS.Service.ModelViews.TrashViews;

namespace DMS.Service.Service
{
    public class StarredService : IStarredService
    {
        private readonly UnitOfWork _unit;

        public StarredService(UnitOfWork _unit)
        {
            this._unit = _unit;
        }
        public async Task<StarredViewModel> GetStarOverviewAsync
            (StarredFilterViewModel model)
        {
            return new StarredViewModel()
            {
                StarFolders = await GetStarFoldersAsync(model),
                StarDocuments = await GetStarDocumentsAsync(model),
            };
            
        }
        public async Task<StarFoldersViewModel> GetStarFoldersAsync(StarredFilterViewModel model)
        {
            model.SearchTerm = model.SearchTerm?.Trim() ?? "";

            IQueryable<Folder> query;

            query = string.IsNullOrWhiteSpace(model.SearchTerm) ?
               _unit.FolderRepository.GetStarred(model.UserId):
               _unit.FolderRepository.StarredSearchAsQueryable(model.SearchTerm, model.UserId);

            int count = query.Count();

            query = SortData(query, model.SortField!, model.SortOrder!);

            List<Folder> folders = await _unit.FolderRepository.GetAllWithPaginationAsync
                (query, model.PageNum, model.PageSize);

            int totalPages = (int)Math.Ceiling((double) count / model.PageSize);

            var starFoldersViewModel = new StarFoldersViewModel()
            {
                StarredFolder = new PagedResult<StarredFolderViewModel>()
                {
                    CurrentPage = model.PageNum,
                    CurrentSearch = model.SearchTerm,
                    SortField = model.SortField,
                    SortOrder = model.SortOrder,
                    TotalPages = totalPages,
                    Items = folders.Select(f => new StarredFolderViewModel()
                    {
                        Id = f.Id,
                        Name = f.Name,
                        AddedAt = f.AddedAt,
                        IsStarred = f.IsStarred,
                        ParentFolderName = f.ParentFolder?.Name,
                        ItemCount = f.Documents?.Count,
                        TotalSize = f.Documents!.Sum(d => d.Size),
                        //TotalSize = _unit.FolderRepository.GetTotalSize(f.Id)
                    }).ToList(),
                }
            };
            return starFoldersViewModel;
        } 
        public async Task<StarDocumentsViewModel> GetStarDocumentsAsync(StarredFilterViewModel model)
        {
            model.SearchTerm = model.SearchTerm?.Trim() ?? "";

            IQueryable<Document> query;

            query = string.IsNullOrWhiteSpace(model.SearchTerm) ?
               _unit.DocumentRepository.GetStarred(model.UserId):
               _unit.DocumentRepository.StarredSearchAsQueryable(model.SearchTerm, model.UserId);

            int count = query.Count();

            query = SortData(query, model.SortField!, model.SortOrder!);

            List<Document> document = await _unit.DocumentRepository.GetAllWithPaginationAsync
                (query, model.PageNum, model.PageSize);

            int totalPages = (int)Math.Ceiling((double) count / model.PageSize);

            var starDocumentsViewModel = new StarDocumentsViewModel()
            {
                StarredDocument = new PagedResult<StarredDocumentViewModel>()
                {
                    CurrentPage = model.PageNum,
                    CurrentSearch = model.SearchTerm,
                    SortField = model.SortField,
                    SortOrder = model.SortOrder,
                    TotalPages = totalPages,
                    Items = document.Select(f => new StarredDocumentViewModel()
                    {
                        Id = f.Id,
                        Name = f.Name,
                        AddedAt = f.AddedAt,
                        IsStarred = f.IsStarred,
                        Size = f.Size,
                        FilePath = f.FilePath,
                        FileType = f.FileType,
                        FolderId = f.FolderId,
                        FolderName = f.Folder!.Name,
                    }).ToList(),
                }
            };
            return starDocumentsViewModel;
        } 

        public bool UnStar<T>(T item) where T : IBaseEntity
        {
            //Folder? folder = await _unit.FolderRepository.GetByIdAsync(folderId);
            try
            {
                item.IsStarred = false;
                if (typeof(T) == typeof(Folder))
                {
                    _unit.FolderRepository.Update(item as Folder);
                }
                else if (typeof(T) == typeof(Document))
                {
                    _unit.DocumentRepository.Update(item as Document);
                }
                _unit.Save();
                return true;
            }catch (Exception ex)
            {
                throw new Exception($"Unstar {typeof(T).Name} can not applied! Try again", ex);
            }
        }

        public async Task<Folder?> GetFolderByIdAsync(string id, string userId)
        {
            Folder? folder = await _unit.FolderRepository.GetByOwnerAsync(id, userId);
            return folder;
        }
        public async Task<Document?> GetDocumentByIdAsync(string id, string userId)
        {
            Document? document = await _unit.DocumentRepository.GetByOwnerAsync(id, userId);
            return document;
        }

        // services helper
        public IQueryable<Folder> SortData
            (IQueryable<Folder> query, string sortField, string sortOrder)
        {
            return (sortField, sortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(f => f.Name),
                ("Name", "desc") => query.OrderByDescending(f => f.Name),

                ("AddedAt", "asc") => query.OrderBy(f => f.AddedAt),
                ("AddedAt", "desc") => query.OrderByDescending(f => f.AddedAt),

                ("TotalSize", "asc") => query.OrderBy(f => f.Documents.Sum(f => f.Size)),
                ("TotalSize", "desc") => query.OrderByDescending(f => f.Documents.Sum(f => f.Size)),

                ("ItemCount", "asc") => query.OrderBy(f => f.Documents!.Count()),
                ("ItemCount", "desc") => query.OrderByDescending(f => f.Documents!.Count()),

                _ => query.OrderByDescending(f => f.AddedAt)
            };
        }
        private IQueryable<Document> SortData
            (IQueryable<Document> query, string sortField, string sortOrder)
        {
            return (sortField, sortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(d => d.Name),
                ("Name", "desc") => query.OrderByDescending(d => d.Name),

                ("AddedAt", "asc") => query.OrderBy(d => d.AddedAt),
                ("AddedAt", "desc") => query.OrderByDescending(d => d.AddedAt),

                ("Size", "asc") => query.OrderBy(d => d.Size),
                ("Size", "desc") => query.OrderByDescending(d => d.Size),

                ("FolderName", "asc") => query.OrderBy(d => d.Folder!.Name),
                ("FolderName", "desc") => query.OrderByDescending(d => d.Folder!.Name),

                _ => query.OrderByDescending(d => d.AddedAt)
            };
        }

    }
}
