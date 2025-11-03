using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMS.Service.ModelViews;

namespace DMS.Service.IService
{
    public interface IDashBoardService
    {
        Task<DashBoardViewModel> GetAdminStats(string adminId);
        Task<DashBoardViewModel> GetUserStats(string userId);
    }
}
