using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API_Computos_Publica.Data.Repository.Interfacez;

namespace API_Computos_Publica.Data.Repository.Clases
{
    public class Repository<T> : IRepository<T> where T : class
    {

        protected readonly DbContext Context;
        internal DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async void AddRange(IEnumerable<T> entities)
        {
            dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await dbSet.AddRangeAsync(entities);
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<T, object>>? include = null, bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public async Task<IEnumerable<T>> GetAllasync(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, Func<IQueryable<T>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<T, object>>? include = null, bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public T GetFirstOrdefault(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<T, object>>? include = null, bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return query.FirstOrDefault();
        }

        public async Task<T> GetFirstOrdefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<T, object>>? include = null, bool asNoTracking = false)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (include != null)
            {
                query = include(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void UpdateG(T entity)
        {
            dbSet.Update(entity);
        }
    }
}
