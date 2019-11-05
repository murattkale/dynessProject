using Core.Entities.Dto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Services
{
    public interface IServiceModel<TEntity> where TEntity : class
    {
        EntityOperationResult<TEntity> Add(TEntity entity);

        EntityOperationResult<TEntity> Update(TEntity entity);

        EntityOperationResult<TEntity> DeleteById(int id);

        int GetCount(Expression<Func<TEntity, bool>> expression = null);

        TEntity Get(Expression<Func<TEntity, bool>> expression = null, params Expression<Func<TEntity, object>>[] includes);

        IEnumerable<TEntity> List(Expression<Func<TEntity, bool>> expression = null, params Expression<Func<TEntity, object>>[] includes);

        EntityPagedDataSource<TEntity> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);
    }
}
