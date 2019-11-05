using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class HesapBilgiManager : IHesapBilgiService
    {
        IEfHesapBilgiData data;

        public HesapBilgiManager(IEfHesapBilgiData data)
        {
            this.data = data;
        }

        public EntityOperationResult<HesapBilgi> Add(HesapBilgi entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<HesapBilgi> Update(HesapBilgi entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<HesapBilgi> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<HesapBilgi, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public HesapBilgi Get(Expression<Func<HesapBilgi, bool>> expression = null, params Expression<Func<HesapBilgi, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<HesapBilgi> List(Expression<Func<HesapBilgi, bool>> expression = null, params Expression<Func<HesapBilgi, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<HesapBilgi> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
