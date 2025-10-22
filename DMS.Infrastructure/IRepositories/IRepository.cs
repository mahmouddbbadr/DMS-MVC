using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositorys
{
    public interface IRepository<TEntity>
    {
        Task<List<TEntity>> GetAllAsync();
        IQueryable<TEntity> GetAllAsQueryable();
        Task<List<TEntity>> GetAllWithPaginationAsync(IQueryable<TEntity> query, int pageNum = 1, int pageSize = 5);
        Task<TEntity?> GetByIdAsync(string id);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        Task DeleteAsync(string id);
        Task<int> GetCountAsync();
    }
}
