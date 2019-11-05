using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Core.Services.Helpers;
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
    public class SmsDurumManager : ISmsDurumService
    {
        IEfSmsDurumData data;

        public SmsDurumManager(IEfSmsDurumData data)
        {
            this.data = data;
        }

        public EntityOperationResult<SmsDurum> Add(SmsDurum entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SmsDurum> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<SmsDurum> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public SmsDurum Get(Expression<Func<SmsDurum, bool>> expression = null, params Expression<Func<SmsDurum, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<SmsDurum, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SmsDurum> List(Expression<Func<SmsDurum, bool>> expression = null, params Expression<Func<SmsDurum, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityOperationResult<SmsDurum> Update(SmsDurum entity)
        {
            throw new NotImplementedException();
        }
    }
}
