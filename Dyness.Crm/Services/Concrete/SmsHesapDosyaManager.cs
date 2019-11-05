using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class SmsHesapDosyaManager : ISmsHesapDosyaService
    {
        IEfSmsHesapDosyaData data;

        public SmsHesapDosyaManager(IEfSmsHesapDosyaData data)
        {
            this.data = data;
        }

        public EntityOperationResult<SmsHesapHareket> Add(SmsHesapHareket entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SmsHesapHareket> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<SmsHesapHareket> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public SmsHesapHareket Get(Expression<Func<SmsHesapHareket, bool>> expression = null, params Expression<Func<SmsHesapHareket, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<SmsHesapHareket, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SmsHesapHareket> List(Expression<Func<SmsHesapHareket, bool>> expression = null, params Expression<Func<SmsHesapHareket, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SmsHesapHareket> Update(SmsHesapHareket entity)
        {
            throw new NotImplementedException();
        }
    }
}
