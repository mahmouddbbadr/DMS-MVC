using AutoMapper;
using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.DocumentViews;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Document = DMS.Domain.Models.Document;

namespace DMS.Service.Service
{
    public class DocumentService : IDocumentService
    {
        private readonly UnitOfWork _unit;
        private readonly IMapper _mapper;
        public DocumentService(UnitOfWork _unit, IMapper mapper)
        {
            this._unit = _unit;
            _mapper = mapper;
        }

        public async Task<DocumentIndexViewModel> GetDocumentsByFolderIdAsync
            (string patentFolderId, string userId)
        {
            var docs = await _unit.DocumentRepository
                .GetDocumentsByFolderIdAsync(patentFolderId, userId);

            Folder? folder = await _unit.FolderRepository.GetByIdAsync(patentFolderId);

            var model = new DocumentIndexViewModel()
            {
                FolderId = patentFolderId,
                FolderName = folder?.Name ?? "UnKnown Folder",
                DocumentList = docs.Select(d => new DocumentListItemViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    FileType = d.FileType,
                    AddedAt = d.AddedAt,
                    IsStarred = d.IsStarred,
                    Size = d.Size,
                    FolderId = d.FolderId
                }).ToList(),
            };

            return model;
        }
        public async Task<DocumentIndexViewModel> GetDocumentsByFolderIdWithPaginationAsync
            (DocumentQueryViewModel modelQuery)
        {
            modelQuery.SearchName = modelQuery.SearchName?.Trim() ?? "";

            IQueryable<Document> query;

            if (!string.IsNullOrWhiteSpace(modelQuery.SearchName))
            {
                query = _unit.DocumentRepository
                    .SearchDocumentByFolderAsQueryable(modelQuery.FolderId, modelQuery.OwnerId, modelQuery.SearchName);
            } else
            {
                query = _unit.DocumentRepository
                .GetDocumentsByFolderIdAsQueryable(modelQuery.FolderId, modelQuery.OwnerId);
            }

            /*
            query = (modelQuery.SortField, modelQuery.SortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(d => d.Name),
                ("Name", "desc") => query.OrderByDescending(d => d.Name),

                ("Size", "asc") => query.OrderBy(d => d.Size),
                ("Size", "desc") => query.OrderByDescending(d => d.Size),

                ("AddedAt", "asc") => query.OrderBy(d => d.AddedAt),
                ("AddedAt", "desc") => query.OrderByDescending(d => d.AddedAt),

                ("Starred", "asc") => query.OrderBy(d => d.IsStarred),
                ("Starred", "desc") => query.OrderByDescending(d => d.IsStarred),

                _ => query.OrderByDescending(d => d.AddedAt)
            };
            */

            query = SortData(query, modelQuery.SortField, modelQuery.SortOrder);
            int totalCount = query.Count();

            List<Document> docs = await _unit.DocumentRepository
                .GetAllWithPaginationAsync(query, modelQuery.PageNum, modelQuery.PageSize);

            Folder? folder = await _unit.FolderRepository.GetByIdAsync(modelQuery.FolderId);

            int total = (int)Math.Ceiling((double)totalCount / modelQuery.PageSize);
            var model = new DocumentIndexViewModel()
            {
                FolderId = modelQuery.FolderId,
                FolderName = folder?.Name ?? "UnKnown Folder",
                CurrentPage = modelQuery.PageNum,
                CurrentSearch = modelQuery.SearchName,
                TotalPages = total,
                //HasNext = modelQuery.PageNum < total,
                //HasPrevious = modelQuery.PageNum > 1,
                SortField = modelQuery.SortField,
                SortOrder = modelQuery.SortOrder,
                DocumentList = docs.Select(d => new DocumentListItemViewModel()
                {
                    Id = d.Id,
                    Name = d.Name,
                    FileType = d.FileType,
                    FilePath = d.FilePath,
                    AddedAt = d.AddedAt,
                    IsStarred = d.IsStarred,
                    Size = d.Size,
                    FolderId = d.FolderId
                }).ToList(),
            };

            return model;
        }

        public async Task DeleteDocumentAsync(string docId, string userId)
        {
            Document? doc = await _unit.DocumentRepository.GetByOwnerAsync(docId, userId);
            if (doc == null)
                throw new Exception("Document Not Found Or You Don't Have Access");
            try
            {
                await _unit.DocumentRepository.DeleteAsync(docId);
                _unit.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error When Deleted", ex);
            }
        }
        public async Task<bool> TrashDocumentAsync(string docId, string userId)
        {
            Document? doc = await _unit.DocumentRepository.GetByOwnerAsync(docId, userId);

            if (doc == null)
                return false;

            doc.IsDeleted = true;
            doc.DeletedAt = DateTime.Now;
            _unit.DocumentRepository.Update(doc);

            _unit.Save();
            return true;
        }
        public async Task<bool> StarDocumentAsync(string docId, string userId, bool isStar)
        {
            Document? doc = await _unit.DocumentRepository.GetByOwnerAsync(docId, userId);

            if (doc != null)
            {
                doc.IsStarred = isStar;
                _unit.DocumentRepository.Update(doc);
                _unit.Save();
                return true;
            }
            return false;
        }
        public async Task<DocumentFileResultViewModel?> GetFileToDownloadAsync
            (string docId, string userId, string wwwroot)
        {
            if (docId != null)
            {
                Document? doc = await _unit.DocumentRepository.GetByOwnerAsync(docId, userId);

                if (doc == null || string.IsNullOrEmpty(doc.FilePath))
                {
                    return null;
                }

                string relativePath = doc.FilePath.TrimStart('/', '\\'); // removes any leading slash
                string filePath = Path.Combine(wwwroot, relativePath.Replace('/', Path.DirectorySeparatorChar));

                if (!File.Exists(filePath))
                {
                    return null;
                }

                byte[] bytes = await File.ReadAllBytesAsync(filePath);

                string contentType = GetContentType(filePath);

                return new DocumentFileResultViewModel()
                {
                    FileBytes = bytes,
                    ContentType = contentType,
                    FileName = doc.Name + Path.GetExtension(filePath)
                };
            }

            return null;
        }

        // Method Helper
        private string GetContentType(string filePath)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        private IQueryable<Document> SortData
            (IQueryable<Document> query, string sortField, string sortOrder)
        {
            return (sortField, sortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(d => d.Name),
                ("Name", "desc") => query.OrderByDescending(d => d.Name),

                ("Size", "asc") => query.OrderBy(d => d.Size),
                ("Size", "desc") => query.OrderByDescending(d => d.Size),

                ("AddedAt", "asc") => query.OrderBy(d => d.AddedAt),
                ("AddedAt", "desc") => query.OrderByDescending(d => d.AddedAt),

                ("Starred", "asc") => query.OrderBy(d => d.IsStarred),
                ("Starred", "desc") => query.OrderByDescending(d => d.IsStarred),

                _ => query.OrderByDescending(d => d.AddedAt)
            };
        }
        public async Task<UploadResult> UploadDocumentAsync(DocumentUploadViewModel model, string wwwroot)
        {
            model.Name = model.Name?.Trim() ?? "";

            if (await _unit.DocumentRepository
                .DocumentNameExistAsync(model.OwnerId, model.FolderId, model.Name))
            {
                return new UploadResult { Success = false, NameExists = true };
            }
            try
            {
                string uploadPath = Path.Combine(wwwroot, "files");

                if(!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);

                string filePath = Path.Combine(uploadPath, uniqueFileName);
            
                using(var fs = new FileStream(filePath, FileMode.Create))
                {
                    await model.File.CopyToAsync(fs);
                }

                Document doc = new()
                {
                    Name = string.IsNullOrEmpty(model.Name) ?
                            Path.GetFileNameWithoutExtension(model.File.FileName) : model.Name,
                    FilePath = Path.Combine("files", uniqueFileName).Replace("\\","/"),
                    FileType = Path.GetExtension(model.File.FileName),
                    AddedAt = DateTime.Now,
                    FolderId = model.FolderId,
                    IsDeleted = false,
                    IsStarred = false,
                    OwnerId = model.OwnerId,
                    Size = (int) (model.File.Length) // Size in Bytes
                };

                await _unit.DocumentRepository.AddAsync(doc);
                _unit.Save();

                return new UploadResult { Success = true };
            }
            catch
            {
                return new UploadResult { Success = false };
            }

        }

        public async Task<Document?> GetDocumentByIdAsync(string docId, string userId)
        {
            if(string.IsNullOrEmpty(docId)) return null;
            return await _unit.DocumentRepository.GetByOwnerAsync(docId, userId);
        }
        public async Task<DocumentUploadViewModel?> SetEditDocumentAsync
            (DocumentEditViewModel model, string userId)
        {           
            Document? doc = await _unit.DocumentRepository.GetByOwnerAsync(model.Id, userId);
            if (doc == null)
                return null;

            return new DocumentUploadViewModel()
            {
                Id = doc.Id,
                Name = doc.Name,
                FolderId = doc.FolderId,
                ExistingFilePath = doc.FilePath,
                ReturnURL = model.ReturnURL,
                OwnerId = userId
            };
        }

        public async Task<bool> EditDocumentAsync(DocumentUploadViewModel model, string wwwroot)
        {
            Document? doc;
            if (model.Id == null) return false;

            doc = await _unit.DocumentRepository.GetByOwnerAsync(model.Id, model.OwnerId);
            if (doc == null) return false;

            doc.Name = string.IsNullOrWhiteSpace(model.Name) ? doc.Name : model.Name;

            if(model.File != null || model?.File?.Length > 0)
            {
                try
                {
                    string uploadPath = Path.Combine(wwwroot, "files");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);

                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fs);
                    }

                    if (!string.IsNullOrEmpty(doc.FilePath))
                    {
                        string oldPath = Path.Combine(wwwroot, doc.FilePath).Replace("/","\\");
                        if(File.Exists(oldPath))
                            File.Delete(oldPath);
                    }

                    doc.FileType = Path.GetExtension(model.File.FileName);
                    doc.FilePath = Path.Combine("files", uniqueFileName).Replace("\\","/");
                    doc.Size = (int)model.File.Length;
                }
                catch
                {
                    return false;
                }
            }

            _unit.DocumentRepository.Update(doc);
            _unit.Save();
            return true;
        }

        public async Task<DocumentDetailsViewModel?> GetDocumentDetailsAsync(string id, string userId)
        {
            Document? doc = await _unit.DocumentRepository.GetByOwnerAsync(id, userId);
            if (doc == null)
                return null;

            return new DocumentDetailsViewModel()
            {
                Id = id,
                Name = doc.Name,
                FilePath = doc.FilePath,
                FileType = doc.FileType,
                IsStarred = doc.IsStarred,
                Size = doc.Size,
                AddedAt = doc.AddedAt,
                FolderId = doc.FolderId,
                FolderName = doc.Folder?.Name ?? "No Folder"
            };
        }

        public async Task<bool> IsAuthorizedFolderAsync(string fId, string userId)
        {
            var folder = await _unit.FolderRepository.GetFolderByIdAuthorizeAsync(fId, userId);
            return folder != null;
        }
        public async Task<bool> EditDocumentModelAsync(DocumentEditModelViewModel model, string wwwroot)
        {
            if (model.Id == null) return false;

            Document? doc;

            doc = await _unit.DocumentRepository.GetByIdAsync(model.Id);
            if (doc == null) return false;

            doc.Name = string.IsNullOrWhiteSpace(model.Name) ? doc.Name : model.Name;

            bool exist = await _unit.DocumentRepository
                .DocumentNameExistAsync(model.OwnerId, doc.FolderId, doc.Name, execludeId: doc.Id);

            if(exist)
                throw new Exception("Document Name Already Exists in the Folder");

            if (model.File != null || model?.File?.Length > 0)
            {
                try
                {
                    string uploadPath = Path.Combine(wwwroot, "files");

                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(model.File.FileName);

                    string filePath = Path.Combine(uploadPath, uniqueFileName);

                    using (var fs = new FileStream(filePath, FileMode.Create))
                    {
                        await model.File.CopyToAsync(fs);
                    }

                    if (!string.IsNullOrEmpty(doc.FilePath))
                    {
                        string oldPath = Path.Combine(wwwroot, doc.FilePath).Replace("/", "\\");
                        if (File.Exists(oldPath))
                            File.Delete(oldPath);
                    }

                    doc.FileType = Path.GetExtension(model.File.FileName);
                    doc.FilePath = Path.Combine("files", uniqueFileName).Replace("\\", "/");
                    doc.Size = (int)model.File.Length;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }

            _unit.DocumentRepository.Update(doc);
            _unit.Save();
            return true;
        }
    }
}
