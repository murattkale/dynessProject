using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class VideoSinifYetkiManager : IVideoSinifYetkiService
    {
        IEfVideoSinifYetkiData data;

        public VideoSinifYetkiManager(IEfVideoSinifYetkiData data)
        {
            this.data = data;
        }

        public EntityOperationResult<VideoSinifYetki> Add(VideoSinifYetki entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoSinifYetki> Update(VideoSinifYetki entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoSinifYetki> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<VideoSinifYetki, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public VideoSinifYetki Get(Expression<Func<VideoSinifYetki, bool>> expression = null, params Expression<Func<VideoSinifYetki, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<VideoSinifYetki> List(Expression<Func<VideoSinifYetki, bool>> expression = null, params Expression<Func<VideoSinifYetki, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<VideoSinifYetki> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
