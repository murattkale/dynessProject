using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class OgrenciSozlesmeTurManager : IOgrenciSozlesmeTurService
    {
        IEfOgrenciSozlesmeTurData data;

        public OgrenciSozlesmeTurManager(IEfOgrenciSozlesmeTurData data)
        {
            this.data = data;
        }

        public EntityOperationResult<OgrenciSozlesmeTur> Add(OgrenciSozlesmeTur entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OgrenciSozlesmeTur> Update(OgrenciSozlesmeTur entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OgrenciSozlesmeTur> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<OgrenciSozlesmeTur, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public OgrenciSozlesmeTur Get(Expression<Func<OgrenciSozlesmeTur, bool>> expression = null, params Expression<Func<OgrenciSozlesmeTur, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<OgrenciSozlesmeTur> List(Expression<Func<OgrenciSozlesmeTur, bool>> expression = null, params Expression<Func<OgrenciSozlesmeTur, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<OgrenciSozlesmeTur> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
