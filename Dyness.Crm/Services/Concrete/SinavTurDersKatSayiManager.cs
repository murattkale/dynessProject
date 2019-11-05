using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class SinavTurDersKatSayiManager : ISinavTurDersKatSayiService
    {
        IEfSinavTurDersKatSayiData data;

        public SinavTurDersKatSayiManager(IEfSinavTurDersKatSayiData data)
        {
            this.data = data;
        }

        public EntityOperationResult<SinavTurDersKatSayi> Add(SinavTurDersKatSayi entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SinavTurDersKatSayi> Update(SinavTurDersKatSayi entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SinavTurDersKatSayi> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<SinavTurDersKatSayi, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public SinavTurDersKatSayi Get(Expression<Func<SinavTurDersKatSayi, bool>> expression = null, params Expression<Func<SinavTurDersKatSayi, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SinavTurDersKatSayi> List(Expression<Func<SinavTurDersKatSayi, bool>> expression = null, params Expression<Func<SinavTurDersKatSayi, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<SinavTurDersKatSayi> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
