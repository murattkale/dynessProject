using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Data
{
    public interface IEntityRepository<TEntity> where TEntity : class , new()
    {
        string Add(TEntity entity);

        string AddOrUpdate(TEntity entity);

        string Update(TEntity entity);

        string DeleteById(int id);

        int GetCount();

        int GetByWhereCaseCount(Expression<Func<TEntity, bool>> where);

        TEntity GetById(int id);

        TEntity GetByWhereCaseIncludeMultipleFirstOrDefault(
            Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAllIncludeMultiple(params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> GetByOrderByTake<TKey>(
            Expression<Func<TEntity, TKey>> orderBy,
            int take,
            bool desceding);

        IEnumerable<TEntity> GetByWhereCase(Expression<Func<TEntity, bool>> where);

        IEnumerable<TEntity> GetByWhereCaseIncludeMultiple(Expression<Func<TEntity, bool>> where,params Expression<Func<TEntity, object>>[] includes);
         
        IEnumerable<TEntity> GetByWhereCaseByOrderByTakeIncludeMultiple<TKey>(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TKey>> orderBy,
            int take,
            bool desceding,
            params Expression<Func<TEntity, object>>[] includes);
    }
}
