using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class VideoVideoKategoriManager : IVideoVideoKategoriService
    {
        IEfVideoVideoKategoriData data;

        public VideoVideoKategoriManager(IEfVideoVideoKategoriData data)
        {
            this.data = data;
        }

        public EntityOperationResult<VideoVideoKategori> Add(VideoVideoKategori entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoVideoKategori> Update(VideoVideoKategori entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<VideoVideoKategori> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<VideoVideoKategori, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public VideoVideoKategori Get(Expression<Func<VideoVideoKategori, bool>> expression = null, params Expression<Func<VideoVideoKategori, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<VideoVideoKategori> List(Expression<Func<VideoVideoKategori, bool>> expression = null, params Expression<Func<VideoVideoKategori, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<VideoVideoKategori> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
