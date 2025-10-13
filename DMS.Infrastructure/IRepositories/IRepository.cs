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
        public List<TEntity> GetAll();
        public IQueryable<TEntity> GetAllAsQuarable();
        public List<TEntity> GetAllWithPagination(IQueryable<TEntity> query, int pageNum = 1, int pageSize = 5);
        public TEntity? GetById(int id);
        public void Add(TEntity entity);
        public void Update(TEntity entity);
        public void Delete(int id);
    }
}
