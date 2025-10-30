using AutoMapper;
using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.Shared;
using Microsoft.AspNetCore.Identity;

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

        public async Task<SharedIndexViewModel>GetSharedByMeAsync
            (
            string userId,
            string search = "",
            string sortOrder = "dateDesc",
            int page = 1,
            int pageSize = 5
            )
        {
            var items = await _unit.SharedItemRepository.GetSharedByMeQueryAsync(
                userId, search, sortOrder, page, pageSize);
            
            var viewModel = _mapper.Map<List<SharedItemViewModel>>(items);
            viewModel.ForEach(r => r.IsSharedByMe = true);

            var sharedIndexViewModel = new SharedIndexViewModel
            {
                SharedByMe = viewModel,
                SearchTerm = search,
                SortOrder = sortOrder,
                CurrentPage = page,
                PageSize = pageSize,
            };

            return (sharedIndexViewModel);



        }
        public async Task<SharedIndexViewModel>GetSharedWithMeAsync(
    string userId,
    string search = "",
    string sortOrder = "dateDesc",
    int page = 1,
    int pageSize = 5)
        {
            var items = await _unit.SharedItemRepository.GetSharedByMeQueryAsync(
        userId, search, sortOrder, page, pageSize);

            var viewModel = _mapper.Map<List<SharedItemViewModel>>(items);
            viewModel.ForEach(r => r.IsSharedByMe = false); 

            var sharedIndexViewModel = new SharedIndexViewModel
            {
                SharedWithMe = viewModel,
                SearchTerm = search,
                SortOrder = sortOrder,
                CurrentPage = page,
                PageSize = pageSize,
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
    }
}
