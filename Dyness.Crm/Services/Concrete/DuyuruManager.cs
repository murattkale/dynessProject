using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class DuyuruManager : IDuyuruService
    {
        IEfDuyuruData data;

        public DuyuruManager(IEfDuyuruData data)
        {
            this.data = data;
        }

        public EntityOperationResult<Duyuru> Add(Duyuru entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<Duyuru> Update(Duyuru entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<Duyuru> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Duyuru Get(Expression<Func<Duyuru, bool>> expression = null, params Expression<Func<Duyuru, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<Duyuru, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Duyuru> List(Expression<Func<Duyuru, bool>> expression = null, params Expression<Func<Duyuru, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<Duyuru> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
