using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace API_Computos_Publica.Data.Repository.Interfacez
{
    public interface IRepository<T> where T : class
    {

        T Get(int id);

        Task<T> GetAsync(int id);

        IEnumerable<T> GetAll(
         Expression<Func<T, bool>>? filter = null,
         Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
         Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, 
         bool asNoTracking = false);

        Task<IEnumerable<T>> GetAllasync(
          Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
           bool asNoTracking = true);

        T GetFirstOrdefault(
           Expression<Func<T, bool>>? filter = null,
          Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
          bool asNoTracking = true
       );

        Task<T> GetFirstOrdefaultAsync(
            Expression<Func<T, bool>>? filter = null,
           Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
           bool asNoTracking = true
        );

        void Add(T entity);

        Task AddAsync(T entity);

        void AddRange(IEnumerable<T> entities);

        Task AddRangeAsync(IEnumerable<T> entities);

        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entities);
        void UpdateG(T entity);

    }
}
