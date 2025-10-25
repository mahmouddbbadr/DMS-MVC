using DMS.Domain.Models;
using DMS.Infrastructure.IRepositorys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public interface ISortSearch<TEntity> where TEntity : class
    {
        public Task<List<TEntity>> GetByNameAsync(string searchName);
        public IQueryable<TEntity> SearchAsQueryable(string searchName);
        public IQueryable<TEntity> SortedByNameAsc();
        public IQueryable<TEntity> SortedByNameDesc();
        public IQueryable<TEntity> SortedByDate();
        public IQueryable<TEntity> SortedByDateDesc();
        IQueryable<TEntity> GetDeleted();
        IQueryable<TEntity> TrashedSearchAsQueryable(string searchName);
        Task<TEntity?> GetDeletedById(string id);
    }
}
