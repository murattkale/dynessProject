using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;

namespace Services.Concrete
{
    public class OgrenciSozlesmeOdemeBilgiSenetImzalayanManager : IOgrenciSozlesmeOdemeBilgiSenetImzalayanService
    {
        IEfOgrenciSozlesmeOdemeBilgiSenetImzalayanData data;

        public OgrenciSozlesmeOdemeBilgiSenetImzalayanManager(IEfOgrenciSozlesmeOdemeBilgiSenetImzalayanData data)
        {
            this.data = data;
        }

        public EntityOperationResult<OgrenciSozlesmeOdemeBilgiSenetImzalayan> Add(OgrenciSozlesmeOdemeBilgiSenetImzalayan entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OgrenciSozlesmeOdemeBilgiSenetImzalayan> Update(OgrenciSozlesmeOdemeBilgiSenetImzalayan entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OgrenciSozlesmeOdemeBilgiSenetImzalayan> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<OgrenciSozlesmeOdemeBilgiSenetImzalayan, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public OgrenciSozlesmeOdemeBilgiSenetImzalayan Get(
            Expression<Func<OgrenciSozlesmeOdemeBilgiSenetImzalayan, bool>> expression = null,
            params Expression<Func<OgrenciSozlesmeOdemeBilgiSenetImzalayan, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<OgrenciSozlesmeOdemeBilgiSenetImzalayan> List(
            Expression<Func<OgrenciSozlesmeOdemeBilgiSenetImzalayan, bool>> expression = null,
            params Expression<Func<OgrenciSozlesmeOdemeBilgiSenetImzalayan, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<OgrenciSozlesmeOdemeBilgiSenetImzalayan> EntityPagedDataSource(
            EntityPagedDataSourceFilter filter,
            IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<OgrenciSozlesmeOdemeBilgiSenetImzalayan>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
