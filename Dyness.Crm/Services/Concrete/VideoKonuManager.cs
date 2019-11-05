using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class VideoKonuManager : IVideoKonuService
    {
        IEfVideoKonuData data;

        public VideoKonuManager(IEfVideoKonuData data)
        {
            this.data = data;
        }

        public EntityOperationResult<VideoKonu> Add(VideoKonu entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoKonu> Update(VideoKonu entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoKonu> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<VideoKonu, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public VideoKonu Get(Expression<Func<VideoKonu, bool>> expression = null, params Expression<Func<VideoKonu, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<VideoKonu> List(Expression<Func<VideoKonu, bool>> expression = null, params Expression<Func<VideoKonu, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<VideoKonu> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
