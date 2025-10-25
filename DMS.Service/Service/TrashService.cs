using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.TrashViews;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            (string? searchName, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            return new TrashViewModel()
            {
                TrashFolders = await GetTrashedFolderAsync
                    (searchName, pageNum, pageSize, sortField, sortOrder),

                TrashDocuments = await GetTrashedDocumentAsync
                    (searchName, pageNum, pageSize, sortField, sortOrder)
            };
        }
        public async Task<TrashFoldersViewModel>GetTrashedFolderAsync
            (string? searchName, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            searchName = searchName?.Trim() ?? "";

            IQueryable<Folder> query;
            if (string.IsNullOrWhiteSpace(searchName))
            {
                query = _unit.FolderRepository.GetDeleted();
            }
            else
            {
                query = _unit.FolderRepository.TrashedSearchAsQueryable(searchName);
            }

            int count = query.Count();

            query = (sortField, sortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(f => f.Name),
                ("Name", "desc") => query.OrderByDescending(f => f.Name),

                ("ItemCount", "asc") => query.OrderBy(f => f.Documents.Count()),
                ("ItemCount", "desc") => query.OrderByDescending(f => f.Documents.Count()),

                ("DeletedAt", "asc") => query.OrderBy(f => f.DeletedAt),
                ("DeletedAt", "desc") => query.OrderByDescending(f => f.DeletedAt),

                _ => query.OrderByDescending(f => f.DeletedAt)
            };

            List<Folder> folders = await _unit.FolderRepository.
                GetAllWithPaginationAsync(query, pageNum, pageSize);

            int totalPages = (int)Math.Ceiling((double)count / pageSize);
            var trashFoldersViewModel = new TrashFoldersViewModel()
            {
                TrashedFolders = new PagedResult<TrashedFolderViewModel>()
                {
                    CurrentPage = pageNum,
                    CurrentSearch = searchName,
                    TotalPages = totalPages,
                    SortField = sortField,
                    SortOrder = sortOrder,
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
            (string? searchName, int pageNum, int pageSize,
            string sortField, string sortOrder)
        {
            searchName = searchName?.Trim() ?? "";

            IQueryable<Document> query;
            if(string.IsNullOrWhiteSpace(searchName))
            {
                query = _unit.DocumentRepository.GetDeleted();
            }else
            {
                query = _unit.DocumentRepository.TrashedSearchAsQueryable(searchName);
            }

            int count = query.Count();

            query = (sortField ,  sortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(d => d.Name),
                ("Name", "desc") => query.OrderByDescending(d => d.Name),

                ("Size", "asc") => query.OrderBy(d => d.Size),
                ("Size", "desc") => query.OrderByDescending(d => d.Size),

                ("DeletedAt", "asc") => query.OrderBy(d => d.DeletedAt),
                ("DeletedAt", "desc") => query.OrderByDescending(d => d.DeletedAt),

                _ => query.OrderByDescending(d => d.DeletedAt)
            };

            List<Document> documents = await _unit.DocumentRepository.
                GetAllWithPaginationAsync(query, pageNum, pageSize);

            int totalPages = (int)Math.Ceiling((double) count / pageSize);
            var trashDocumentsViewModel = new TrashDocumentsViewModel()
            {
                TrashedDocuments = new PagedResult<TrashedDocumentViewModel>()
                {
                    CurrentPage = pageNum,
                    CurrentSearch = searchName,
                    TotalPages = totalPages,
                    SortField = sortField,
                    SortOrder = sortOrder,
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

        public async Task<Folder?> GetFolderByIdAsync(string id)
        {
            Folder? folder = await _unit.FolderRepository.GetDeletedById(id);
            return folder;
        }
        public async Task<Document?> GetDocumentByIdAsync(string id)
        {
            Document? document = await _unit.DocumentRepository.GetDeletedById(id);
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
        public async Task<bool> DeleteFolderAsync(string folderId)
        {
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
        public async Task<bool> DeleteDocumentAsync(string documentId)
        {
            try
            {
                await _unit.DocumentRepository.DeleteAsync(documentId);
                _unit.Save();
                return true;
            }catch (Exception ex)
            {
                throw new Exception("Delete Document can not applied! Try again", ex);
            }
        }
    }
}
