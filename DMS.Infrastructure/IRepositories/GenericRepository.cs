using DMS.Domain.Models;
using DMS.Infrastructure.DataContext;
using DMS.Infrastructure.IRepositorys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.IRepositories
{
    public class GenericRepository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        public readonly DMSContext context;
        public DbSet<TEntity> _dbset;

        public GenericRepository(DMSContext context)
        {
            this.context = context;
            _dbset = context.Set<TEntity>();
        }
        public List<TEntity> GetAll()
        {
            return [.. _dbset.AsNoTracking()];
        }
        public IQueryable<TEntity> GetAllAsQuarable()
        {
            return _dbset.AsNoTracking();
        }
        public List<TEntity> GetAllWithPagination
            (IQueryable<TEntity> query,int pageNum = 1, int pageSize = 5)
        {
           return [..query.Skip((pageNum - 1) * pageSize).Take(pageSize)];
        }
        public TEntity? GetById(string id)
        {
            return _dbset.Find(id);
        }
        public void Add(TEntity entity)
        {
            _dbset.Add(entity);
        }
        public void Update(TEntity entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            //_dbset.Update(entity);
        }
        public void Delete(string id)
        {
            TEntity? entity = GetById(id);
            if (entity != null)
            {
                _dbset.Remove(entity);
            }
        }
    }
}
