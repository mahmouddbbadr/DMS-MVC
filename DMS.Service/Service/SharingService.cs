using AutoMapper;
using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.DocumentViews;
using DMS.Service.ModelViews.Shared;
using Microsoft.AspNetCore.Identity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DMS.Service.Service
{
    public class SharingService: ISharingService
    {
        private readonly UnitOfWork _unit;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public SharingService(UnitOfWork _unit, IMapper mapper, UserManager<AppUser> userManager)
        {
            this._unit = _unit;
            this._mapper = mapper;
            this._userManager = userManager;
        }

        public async Task<SharedIndexViewModel>GetSharedByMeAsync(
            string userId,
            string search,
            string sortOrder,
            int page,
            int pageSize)
        {
            var query =  _unit.SharedItemRepository.GetSharedByMeQuery(
                userId, search, sortOrder, page, pageSize);
            
            int count = query.Count();
            int totalPages = (int)Math.Ceiling((double)count / pageSize);

            var items = await _unit.SharedItemRepository
                .GetAllWithPaginationAsync(query, page, pageSize);

            var viewModel = _mapper.Map<List<SharedItemViewModel>>(items);
            viewModel.ForEach(r => r.IsSharedByMe = true);

            var sharedIndexViewModel = new SharedIndexViewModel
            {
                SharedByMe = viewModel,
                SearchTerm = search,
                SortOrder = sortOrder,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPagesByMe = totalPages
            };

            return (sharedIndexViewModel);
        }
        public async Task<SharedIndexViewModel>GetSharedWithMeAsync(
            string userId,
            string search,
            string sortOrder,
            int page,
            int pageSize)
        {
            var query = _unit.SharedItemRepository.GetSharedWithMeQuery(
                userId, search, sortOrder, page, pageSize);

            int count = query.Count();
            int totalPages = (int)Math.Ceiling((double)count / pageSize);

            var items = await _unit.SharedItemRepository
                .GetAllWithPaginationAsync(query, page, pageSize);

            var viewModel = _mapper.Map<List<SharedItemViewModel>>(items);
            viewModel.ForEach(r => r.IsSharedByMe = false); 

            var sharedIndexViewModel = new SharedIndexViewModel
            {
                SharedWithMe = viewModel,
                SearchTerm = search,
                SortOrder = sortOrder,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPagesWithMe = totalPages
            };

            return sharedIndexViewModel;
        }


        public async Task ShareDocumentAsync(ShareViewModel model)
        {
            var sharedWithUser = await _userManager.FindByEmailAsync(model.SharedWithUserEmail);
            if (string.IsNullOrWhiteSpace(model.SharedWithUserEmail) ||
                !new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(model.SharedWithUserEmail))
            {
                throw new ArgumentException("Invalid email format.");
            }
            if (sharedWithUser == null)
            {
                throw new Exception("No user found with this email address.");
            }
            var document = await _unit.DocumentRepository.GetByIdAsync(model.DocumentId);
            if (document == null)
            {
                throw new Exception("Document not found.");
            }

            if (document.Folder?.OwnerId != model.SharedByUserId)
            {
                throw new Exception("You can only share documents that you own.");
            }


            var alreadyShared = await _unit.SharedItemRepository
        .AnyAsync(s => s.DocumentId == model.DocumentId && s.SharedWithUserId == sharedWithUser.Id);

            if (alreadyShared)
            {
                throw new Exception("This document is already shared with this user.");
            }

            var sharedItem = _mapper.Map<SharedItem>(model);
            sharedItem.SharedWithUserId = sharedWithUser.Id;
            sharedItem.AddedAt = DateTime.UtcNow;
            sharedItem.FolderId = null;
            await _unit.SharedItemRepository.AddAsync(sharedItem);
            _unit.Save();
        }

        public async Task ShareFolderAsync(ShareViewModel model)
        {
            // Validate email format
            if (string.IsNullOrWhiteSpace(model.SharedWithUserEmail) ||
                !new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(model.SharedWithUserEmail))
            {
                throw new ArgumentException("Invalid email format.");
            }

            // Find user by email
            var sharedWithUser = await _userManager.FindByEmailAsync(model.SharedWithUserEmail);
            if (sharedWithUser == null)
            {
                throw new Exception("No user found with this email address.");
            }

            // Get folder - FIX: Use FolderRepository instead of SharedItemRepository
            var folder = await _unit.FolderRepository.GetByIdAsync(model.FolderId);
            if (folder == null)
            {
                throw new Exception("Folder not found.");
            }

            // Check ownership
            if (folder.OwnerId != model.SharedByUserId)
            {
                throw new Exception("You can only share folders that you own.");
            }

            // Check if already shared - FIX: Added await
            var alreadyShared = await _unit.SharedItemRepository
                .AnyAsync(s => s.FolderId == model.FolderId && s.SharedWithUserId == sharedWithUser.Id);

            if (alreadyShared)
            {
                throw new Exception("This folder is already shared with this user.");
            }

            // Create shared item
            var sharedItem = _mapper.Map<SharedItem>(model);
            sharedItem.SharedWithUserId = sharedWithUser.Id;
            sharedItem.AddedAt = DateTime.UtcNow;
            sharedItem.DocumentId = null;

            await _unit.SharedItemRepository.AddAsync(sharedItem);
            _unit.Save();
        }
        public async Task<int> UnshareAllAsync(string itemId, string itemType, string currentUserId)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(itemId))
                throw new ArgumentException("Item ID is required.");

            if (itemType != "document" && itemType != "folder")
                throw new ArgumentException("Invalid item type.");

            // Get shared items from repository
            var sharedItems = await _unit.SharedItemRepository
                .GetSharedItemsByUserAndItemAsync(itemId, itemType, currentUserId);

            if (!sharedItems.Any())
                throw new Exception($"No shared users found for this {itemType}.");

            // Delete all shared items
            foreach (var item in sharedItems)
            {
                await _unit.SharedItemRepository.DeleteAsync(item.Id);
            }

            _unit.Save();

            return sharedItems.Count;
        }
        public async Task UnshareAsync(string shareItemId)
        {
            var sharedItem = await _unit.SharedItemRepository.GetByIdAsync(shareItemId);
            if (sharedItem == null)
                throw new Exception("Shared item not found.");

            await _unit.SharedItemRepository.DeleteAsync(sharedItem.Id);
            _unit.Save();
        }
        public async Task<string> GetUserEmailAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user?.Email ?? "Unknown User";
        }

        public async Task<List<string>> GetUserEmailsAsync(List<string> userIds)
        {
            var emails = new List<string>();

            foreach (var userId in userIds)
            {
                var email = await GetUserEmailAsync(userId);
                emails.Add(email);
            }

            return emails;
        }

        private async Task<string> GetItemNameAsync(string itemId, string type)
        {
            if (type.ToLower() == "folder")
            {
                var folder = await _unit.FolderRepository.GetByIdAsync(itemId);
                return folder?.Name ?? "Unknown Folder";
            }
            else
            {
                var document = await _unit.DocumentRepository.GetByIdAsync(itemId);
                return document?.Name ?? "Unknown Document";
            }
        }

        private async Task<List<SharedUserViewModel>> MapSharedItemsToUserViewModels(List<SharedItem> sharedItems)
        {
            var userViewModels = new List<SharedUserViewModel>();

            foreach (var sharedItem in sharedItems)
            {
                var userViewModel = _mapper.Map<SharedUserViewModel>(sharedItem);

                var user = await _userManager.FindByIdAsync(sharedItem.SharedWithUserId);
                userViewModel.Email = user?.Email ?? "Unknown User";

                userViewModels.Add(userViewModel);
            }

            return userViewModels;
        }
        public async Task<UnshareSelectionViewModel> GetSharedUsersAsync(string itemId, string type, string currentUserId)
        {
            var model = new UnshareSelectionViewModel
            {
                ItemId = itemId,
                Type = type.ToLower(),
                ItemName = await GetItemNameAsync(itemId, type)
            };

            var sharedItems = await _unit.SharedItemRepository
                .GetSharedItemsByUserAndItemAsync(itemId, type, currentUserId);

            model.SharedUsers = await  MapSharedItemsToUserViewModels(sharedItems);

            return model;
        }

        public async Task<int> UnshareWithUsersAsync(string itemId, string type, List<string> userIds, string currentUserId)
        {
            if (userIds == null || !userIds.Any())
                return 0;

            int unsharedCount = 0;

            foreach (var userId in userIds)
            {
                var success = await UnshareWithUserAsync(itemId, type, userId, currentUserId);
                if (success)
                {
                    unsharedCount++;
                }
            }

            return unsharedCount;
        }

        public async Task<bool> UnshareWithUserAsync(string itemId, string type, string userId, string currentUserId)
        {
            SharedItem sharedItem = null;

            if (type.ToLower() == "folder")
            {
                sharedItem = await _unit.SharedItemRepository
                    .FirstOrDefaultAsync(s => s.FolderId == itemId &&
                                            s.SharedWithUserId == userId &&
                                            s.SharedByUserId == currentUserId);
            }
            else
            {
                sharedItem = await _unit.SharedItemRepository
                    .FirstOrDefaultAsync(s => s.DocumentId == itemId &&
                                            s.SharedWithUserId == userId &&
                                            s.SharedByUserId == currentUserId);
            }

            if (sharedItem != null)
            {
                await _unit.SharedItemRepository.DeleteAsync(sharedItem.Id);
                _unit.Save();
                return true;
            }

            return false;
        }
    
        public async Task<bool> IsAuthorizedSharedFolderAsync(string fId, string userId)
        {
            var sharedFolder = await _unit.SharedItemRepository
                .GetSharedFolderByIdAuthorizeAsync(fId, userId);

            return sharedFolder != null;
        }

        public async Task<DocumentIndexViewModel> GetDocumentsBySharedFolderIdWithPaginationAsync
            (FolderSharedViewModel modelQuery)
        {
            modelQuery.SearchName = modelQuery.SearchName?.Trim() ?? "";

            IQueryable<Document> query;

            if (!string.IsNullOrWhiteSpace(modelQuery.SearchName))
            {
                query = _unit.DocumentRepository
                    .SearchDocumentsBySharedFolderAsQueryable(modelQuery.FolderId, modelQuery.SearchName);
            }
            else
            {
                query = _unit.DocumentRepository
                .GetDocumentsBySharedFolderIdAsQueryable(modelQuery.FolderId);
            }

            query = SortData(query, modelQuery.SortField, modelQuery.SortOrder);
            int totalCount = query.Count();

            List<Document> docs = await _unit.DocumentRepository
                .GetAllWithPaginationAsync(query, modelQuery.PageNum, modelQuery.PageSize);

            Folder? folder = await _unit.FolderRepository.GetByIdAsync(modelQuery.FolderId);
            SharedItem? sharedItem = await _unit.SharedItemRepository
                .GetByIdAsync(modelQuery.FolderId);

            int total = (int)Math.Ceiling((double)totalCount / modelQuery.PageSize);
            var model = new DocumentIndexViewModel()
            {
                FolderId = modelQuery.FolderId,
                FolderName = folder?.Name ?? "UnKnown Folder",
                CurrentPage = modelQuery.PageNum,
                CurrentSearch = modelQuery.SearchName,
                TotalPages = total,
                SortField = modelQuery.SortField,
                SortOrder = modelQuery.SortOrder,
                Permission = sharedItem?.PermissionLevel,
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
    }
}
