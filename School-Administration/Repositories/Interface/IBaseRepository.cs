using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace School_Administration.Repositories.Interface
{
    public interface IBaseRepository<TEntity>
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        Task<IEnumerable<TEntity>> GetAllEntity(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);

        public Task<TEntity> GetById(object id);

        Task Delete(object id);
    }
}
