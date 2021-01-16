using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UbiClub.Feedback.Data.Interfaces;
using UbiClub.Feedback.Entities;

namespace UbiClub.Feedback.Data
{
    public class GenericRepository : IGenericRepository
    {
        private readonly FeedbackContext _context;
        public GenericRepository(FeedbackContext context)
        {
            _context = context;
        }
        public IQueryable<TEntity> GetQuery<TEntity>(bool noTracking = true) where TEntity : BaseEntity
        {
            return noTracking ? _context.Set<TEntity>().AsNoTracking() : _context.Set<TEntity>();
        }

        public async Task<List<TEntity>> GetListAsync<TEntity>(bool noTracking = true) where TEntity : BaseEntity
        {
            return await GetQuery<TEntity>(noTracking).ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(Guid id, bool noTracking = true) where TEntity : BaseEntity
        {
            return await GetQuery<TEntity>(noTracking).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter, bool noTracking = true) where TEntity : BaseEntity
        {
            return await GetQuery<TEntity>(noTracking).FirstOrDefaultAsync(filter);
        }

        public async Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter, string includeProperties,
            bool noTracking = true) where TEntity : BaseEntity
        {
            var query = GetQuery<TEntity>(noTracking);
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return await query.FirstOrDefaultAsync(filter);
        }

        public void Add<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _context.Set<TEntity>().Add(entity);
        }

        public void AddRange<TEntity>(List<TEntity> entities) where TEntity : BaseEntity
        {
            _context.Set<TEntity>().AddRange(entities);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Attach<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            if (_context.ChangeTracker.Entries<TEntity>().Any(e => e.Entity.Id == entity.Id))
            {
                return;
            }
            _context.Set<TEntity>().Attach(entity);
        }
        public void SetIsModified<TEntity>(TEntity entity, bool isModified, params string[] propertyNames) where TEntity : BaseEntity
        {
            foreach (var propertyName in propertyNames)
            {
                _context.Entry(entity).Property(propertyName).IsModified = isModified;
            }
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            _context.Entry(entity).State = EntityState.Deleted;
        }

        public void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : BaseEntity
        {
            IEnumerable<TEntity> records = GetQuery<TEntity>().Where(criteria);
            foreach (TEntity record in records)
            {
                Delete<TEntity>(record);
            }
        }

        public void RemoveRange<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : BaseEntity
        {
            _context.Set<TEntity>().RemoveRange(_context.Set<TEntity>().Where(criteria));
        }

        public async Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : BaseEntity
        {
            return criteria == null ? await GetQuery<TEntity>().CountAsync() : await GetQuery<TEntity>().CountAsync(criteria);
        }

        public async Task<int> SaveAsync()
        {
            foreach (var entry in _context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added))
            {
                if (!(entry.Entity is BaseEntity entity)) 
                    continue;

                entity.CreatedDate = DateTimeOffset.UtcNow;
            }

            return await _context.SaveChangesAsync();
        }
	}
}