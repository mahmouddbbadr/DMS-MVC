using AutoMapper;
using DMS.Domain.Models;
using DMS.Infrastructure.UnitOfWorks;
using DMS.Service.IService;
using DMS.Service.ModelViews.SharedViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        public async Task<(List<SharedItemViewModel> Items, int TotalCount)> GetSharedByMeAsync
            (
            string userId,
            string search = "",
            string sortOrder = "dateDesc",
            int page = 1,
            int pageSize = 5
            )
        {

            var query = _unit.SharedItemRepository.GetAllAsQueryable()
                         .Where(s => s.SharedWithUserId == userId);
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(s =>
                    (s.Document != null && s.Document.Name.ToLower().Contains(search)) ||
                    (s.Folder != null && s.Folder.Name.ToLower().Contains(search)) ||
                    (s.SharedWithUser != null && s.SharedWithUser.UserName.ToLower().Contains(search))
                );
            }


            query = sortOrder switch
            {
                "nameAsc" => query.OrderBy(s => s.Document!.Name ?? s.Folder!.Name),
                "nameDesc" => query.OrderByDescending(s => s.Document!.Name ?? s.Folder!.Name),
                "dateAsc" => query.OrderBy(s => s.AddedAt),
                "dateDesc" => query.OrderByDescending(s => s.AddedAt),
                _ => query.OrderByDescending(s => s.AddedAt)
            };


            var totalCount = await query.CountAsync();

            var items = await _unit.SharedItemRepository.GetAllWithPaginationAsync(query, page, pageSize);

            var viewModel = _mapper.Map<List<SharedItemViewModel>>(items);
            viewModel.ForEach(r => r.IsSharedByMe = true);

            return (viewModel,totalCount);



        }
        public async Task<(List<SharedItemViewModel> Items, int TotalCount)> GetSharedWithMeAsync(
    string userId,
    string search = "",
    string sortOrder = "dateDesc",
    int page = 1,
    int pageSize = 5)
        {
            var query = _unit.SharedItemRepository.GetAllAsQueryable()
                          .Where(s => s.SharedWithUserId == userId);

            // Search
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(s =>
                    (s.Document != null && s.Document.Name.ToLower().Contains(search)) ||
                    (s.Folder != null && s.Folder.Name.ToLower().Contains(search)) ||
                    (s.SharedByUser != null && s.SharedByUser.UserName.ToLower().Contains(search))
                );
            }

            // Sort
            query = sortOrder switch
            {
                "nameAsc" => query.OrderBy(s => s.Document!.Name ?? s.Folder!.Name),
                "nameDesc" => query.OrderByDescending(s => s.Document!.Name ?? s.Folder!.Name),
                "dateAsc" => query.OrderBy(s => s.AddedAt),
                "dateDesc" => query.OrderByDescending(s => s.AddedAt),
                _ => query.OrderByDescending(s => s.AddedAt)
            };

            var totalCount = await query.CountAsync();

            var items = await _unit.SharedItemRepository.GetAllWithPaginationAsync(query, page, pageSize);

            var viewModel = _mapper.Map<List<SharedItemViewModel>>(items);
            viewModel.ForEach(r => r.IsSharedByMe = false); 

            return (viewModel, totalCount);
        }


        public async Task ShareDocumentAsync(ShareViewModel model)
        {
            var sharedWithUser = await _userManager.FindByEmailAsync(model.SharedWithUserEmail);
            if (sharedWithUser == null)
            {
                throw new Exception("User not found with this email.");
            }
            var sharedItem = _mapper.Map<SharedItem>(model);
            sharedItem.Id = Guid.NewGuid().ToString();
            sharedItem.SharedWithUserId = sharedWithUser.Id;
            sharedItem.AddedAt = DateTime.UtcNow;
            sharedItem.FolderId = null;
            await _unit.SharedItemRepository.AddAsync(sharedItem);
            _unit.Save();
        }

        public async Task ShareFolderAsync(ShareViewModel model)
        {
            var sharedWithUser= await _userManager.FindByEmailAsync(model.SharedWithUserEmail);
            if (sharedWithUser == null)
            {
                throw new Exception("User not found with this email.");
            }

            var sharedItem=_mapper.Map<SharedItem>(model);
            sharedItem.Id = Guid.NewGuid().ToString();
            sharedItem.SharedWithUserId = sharedWithUser.Id;
            sharedItem.AddedAt = DateTime.UtcNow;
            sharedItem.DocumentId = null;
            await _unit.SharedItemRepository.AddAsync(sharedItem);
            _unit.Save();

            }

        public async Task UnshareAsync(string sharedItemId)
        {
            var sharedItem = await _unit.SharedItemRepository.GetByIdAsync(sharedItemId);
            if (sharedItem == null)
                throw new Exception("Shared item not found.");
          await _unit.SharedItemRepository.DeleteAsync(sharedItem.Id);
            _unit.Save();
        }
    }
}
