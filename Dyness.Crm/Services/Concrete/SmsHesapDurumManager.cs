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
    public class SmsHesapDurumManager : ISmsHesapDurumService
    {
        IEfSmsHesapDurumData data;

        public SmsHesapDurumManager(IEfSmsHesapDurumData data)
        {
            this.data = data;
        }

        public EntityOperationResult<SmsHesapDurum> Add(SmsHesapDurum entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SmsHesapDurum> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<SmsHesapDurum> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public SmsHesapDurum Get(Expression<Func<SmsHesapDurum, bool>> expression = null, params Expression<Func<SmsHesapDurum, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<SmsHesapDurum, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SmsHesapDurum> List(Expression<Func<SmsHesapDurum, bool>> expression = null, params Expression<Func<SmsHesapDurum, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityOperationResult<SmsHesapDurum> Update(SmsHesapDurum entity)
        {
            throw new NotImplementedException();
        }
    }
}
