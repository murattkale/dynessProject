using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class OkulTurManager : IOkulTurService
    {
        IEfOkulTurData data;

        public OkulTurManager(IEfOkulTurData data)
        {
            this.data = data;
        }

        public EntityOperationResult<OkulTur> Add(OkulTur entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OkulTur> Update(OkulTur entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OkulTur> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<OkulTur, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public OkulTur Get(Expression<Func<OkulTur, bool>> expression = null, params Expression<Func<OkulTur, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OkulTur> List(Expression<Func<OkulTur, bool>> expression = null, params Expression<Func<OkulTur, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<OkulTur> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
