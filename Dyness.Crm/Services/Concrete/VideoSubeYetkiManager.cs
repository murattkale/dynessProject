using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class VideoSubeYetkiManager : IVideoSubeYetkiService
    {
        IEfVideoSubeYetkiData data;

        public VideoSubeYetkiManager(IEfVideoSubeYetkiData data)
        {
            this.data = data;
        }

        public EntityOperationResult<VideoSubeYetki> Add(VideoSubeYetki entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoSubeYetki> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoSubeYetki> Update(VideoSubeYetki entity)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<VideoSubeYetki, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public VideoSubeYetki Get(Expression<Func<VideoSubeYetki, bool>> expression = null, params Expression<Func<VideoSubeYetki, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<VideoSubeYetki> List(Expression<Func<VideoSubeYetki, bool>> expression = null, params Expression<Func<VideoSubeYetki, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<VideoSubeYetki> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
