using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UbiClub.Feedback.Entities;

namespace UbiClub.Feedback.Data.Interfaces
{
    public interface IGenericRepository
        {
            IQueryable<TEntity> GetQuery<TEntity>(bool noTracking = true) where TEntity : BaseEntity;


            Task<TEntity> GetByIdAsync<TEntity>(Guid id, bool noTracking = true) where TEntity : BaseEntity;

            Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter, bool noTracking = true) where TEntity : BaseEntity;

            Task<TEntity> GetFirstAsync<TEntity>(Expression<Func<TEntity, bool>> filter, string includeProperties,
                bool noTracking = true) where TEntity : BaseEntity;


            void Add<TEntity>(TEntity entity) where TEntity : BaseEntity;

            void AddRange<TEntity>(List<TEntity> entities) where TEntity : BaseEntity;

            void Update<TEntity>(TEntity entity) where TEntity : BaseEntity;

            void Attach<TEntity>(TEntity entity) where TEntity : BaseEntity;

            void RemoveRange<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : BaseEntity;

            void Delete<TEntity>(TEntity entity) where TEntity : BaseEntity;

            void Delete<TEntity>(Expression<Func<TEntity, bool>> criteria) where TEntity : BaseEntity;

            Task<int> CountAsync<TEntity>(Expression<Func<TEntity, bool>> criteria = null) where TEntity : BaseEntity;


            void SetIsModified<TEntity>(TEntity entity, bool isModified, params string[] propertyNames) where TEntity : BaseEntity;

            Task<int> SaveAsync();

    }
}