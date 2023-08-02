using Microsoft.EntityFrameworkCore;
using School_Administration.Data;
using School_Administration.Repositories.Interface;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace School_Administration.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly SchoolDbContext _context;

        internal DbSet<TEntity> DbSet;

        public BaseRepository(SchoolDbContext context)
        {
            _context = context;

            DbSet = _context.Set<TEntity>();
        }

        public void Add(TEntity entity)
        {
                DbSet.Add(entity);            
        }
        public async Task<IEnumerable<TEntity>> GetAllEntity(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            IQueryable<TEntity> results = DbSet.AsNoTracking();

            if (predicate != null)
                results = results.Where(predicate);

            if (includes != null)
                results = includes(results);

            return await results.ToListAsync();
        }

        public void Update(TEntity entity)
        {
            DbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<TEntity> GetById(object id)
        {
            var entryToFind = await DbSet.FindAsync(id);

            if (entryToFind != null)
                _context.Entry(entryToFind).State = EntityState.Detached;

            return entryToFind;
        }

        public async Task Delete(object id)
        {
            var entity = await GetById(id);
            DbSet.Remove(entity);
        }

    }
}
