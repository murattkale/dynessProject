using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class VideoKurumYetkiManager : IVideoKurumYetkiService
    {
        IEfVideoKurumYetkiData data;

        public VideoKurumYetkiManager(IEfVideoKurumYetkiData data)
        {
            this.data = data;
        }

        public EntityOperationResult<VideoKurumYetki> Add(VideoKurumYetki entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoKurumYetki> Update(VideoKurumYetki entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoKurumYetki> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<VideoKurumYetki, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public VideoKurumYetki Get(Expression<Func<VideoKurumYetki, bool>> expression = null, params Expression<Func<VideoKurumYetki, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<VideoKurumYetki> List(Expression<Func<VideoKurumYetki, bool>> expression = null, params Expression<Func<VideoKurumYetki, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<VideoKurumYetki> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
