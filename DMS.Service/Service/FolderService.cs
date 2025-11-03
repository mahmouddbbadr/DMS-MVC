using AutoMapper;
using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.DocumentViews;
using DMS.Service.ModelViews.Folder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Service.Service
{
    public class FolderService: IFolderService
    {
        private readonly UnitOfWork _unit;
        private readonly IMapper _mapper;

        public FolderService(UnitOfWork _unit, IMapper mapper)
        {
            this._unit = _unit;
            this._mapper = mapper;
        }

        public async Task<FolderIndexViewModel> GetFoldersWithPaginationAsync
            (FolderQueryViewModel modelQuery)
        {
            modelQuery.SearchName = modelQuery.SearchName?.Trim() ?? "";

            IQueryable<Folder> query;

            if (!string.IsNullOrWhiteSpace(modelQuery.SearchName))
            {
                query = _unit.FolderRepository
                    .SearchFoldersAsQueryable(modelQuery.ParentId, modelQuery.OwnerId, modelQuery.SearchName);
            }
            else
            {
                query = _unit.FolderRepository
                .GetFoldersAsQueryable(modelQuery.ParentId, modelQuery.OwnerId);
            }

            query = SortData(query, modelQuery.SortField, modelQuery.SortOrder);

            int totalCount = query.Count();

            List<Folder> folders = await _unit.FolderRepository
                .GetAllWithPaginationAsync(query, modelQuery.PageNum, modelQuery.PageSize);

            //if(modelQuery.ParentId  != null)
            //{
            //    Folder? folder = await _unit.FolderRepository
            //        .GetByIdAsync(modelQuery.ParentId);
            //}

            int total = (int)Math.Ceiling((double)totalCount / modelQuery.PageSize);
            var model = new FolderIndexViewModel()
            {
                CurrentPage = modelQuery.PageNum,
                CurrentSearch = modelQuery.SearchName,
                TotalPages = total,
                HasNext = modelQuery.PageNum < total,
                HasPrevious = modelQuery.PageNum > 1,
                SortField = modelQuery.SortField,
                SortOrder = modelQuery.SortOrder,
                FolderList = folders.Select(f => new FolderListItemViewModel()
                {
                    Id = f.Id,
                    Name = f.Name,
                    AddedAt = f.AddedAt,
                    IsStarred = f.IsStarred,
                    ItemCount = f.Documents?.Count() ?? 0,
                    TotalSize = f.Documents!.Sum(f => f.Size),
                }).ToList(),
            };

            return model;
        }

        public IQueryable<Folder> SortData
            (IQueryable<Folder> query, string sortField, string sortOrder)
        {
            return (sortField, sortOrder?.ToLower()) switch
            {
                ("Name", "asc") => query.OrderBy(f => f.Name),
                ("Name", "desc") => query.OrderByDescending(f => f.Name),

                ("AddedAt", "asc") => query.OrderBy(f => f.AddedAt),
                ("AddedAt", "desc") => query.OrderByDescending(f => f.AddedAt),

                ("TotalSize", "asc") => query.OrderBy(f => f.Documents!.Sum(d => d.Size)),
                ("TotalSize", "desc") => query.OrderByDescending(f => f.Documents!.Sum(d => d.Size)),

                ("Starred", "asc") => query.OrderBy(f => f.IsStarred),
                ("Starred", "desc") => query.OrderByDescending(f => f.IsStarred),

                ("ItemCount", "asc") => query.OrderBy(f => f.Documents!.Count()),
                ("ItemCount", "desc") => query.OrderByDescending(f => f.Documents!.Count()),

                _ => query.OrderByDescending(f => f.AddedAt)
            };
        }

        public async Task<CreateFolderResult> CreateAsync(FolderCreateViewModel model)
        {
            model.Name = model.Name.Trim();

            if (await _unit.FolderRepository.FolderNameExistAsync(model.OwnerId, model.Name))
            {
                return new CreateFolderResult
                {
                    Success = false,
                    NameExists = true
                };
            }

            Folder folder = new()
            {
                Name = model.Name,
                OwnerId = model.OwnerId,
                AddedAt = DateTime.Now,
                IsDeleted = false,
                IsStarred = false
            };

            try
            {
                await _unit.FolderRepository.AddAsync(folder);
                _unit.Save();

                return new CreateFolderResult { Success = true };
            }
            catch (Exception ex)
            {
                return new CreateFolderResult
                {
                    Success = false,
                    ErrorMessage = "Error creating folder. Please try again later."
                };
            }
        }


        public async Task<Folder?> GetFolderAsync(string folId, string userId)
        {
            return await _unit.FolderRepository.GetByOwnerAsync(folId, userId);
        }
        
        public async Task<bool> RenameFolderAsync(FolderEditViewModel model, string userId)
        {
            if(await _unit.FolderRepository.FolderNameExistAsync(userId, model.Name, execludeId: model.Id))
            {
                throw new Exception("Folder name already exists.");
            }

            Folder? fol = await _unit.FolderRepository.GetByOwnerAsync(model.Id, userId);
            if (fol != null)
            {
                try
                {
                    fol.Name = model.Name;
                    _unit.FolderRepository.Update(fol);
                    _unit.Save();
                }catch(Exception ex)
                {
                    throw new Exception("Error renaming folder. Please try again later.");
                }
                return true;
            }
            return false;
        }
        public async Task<bool> StarFolderAsync(string folId, string userId, bool isStar)
        {
            Folder? fol = await _unit.FolderRepository.GetByOwnerAsync(folId, userId);

            if (fol != null)
            {
                fol.IsStarred = isStar;
                _unit.FolderRepository.Update(fol);
                _unit.Save();
                return true;
            }
            return false;
        }

        public async Task<bool> TrashFolderAsync(string folId, string userId)
        {
            Folder? fol = await _unit.FolderRepository.GetByOwnerAsync(folId, userId);

            if (fol == null)
                return false;

            fol.IsDeleted = true;
            fol.DeletedAt = DateTime.Now;
            _unit.FolderRepository.Update(fol);

            if(fol.Documents == null || fol.Documents.Count == 0)
            {
                _unit.Save();
                return true;
            }

            foreach (var doc in fol.Documents)
            {
                doc.IsDeleted = true;
                doc.DeletedAt = DateTime.Now;
                _unit.DocumentRepository.Update(doc);
            }

            _unit.Save();
            return true;
        }
    }
}
