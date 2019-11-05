using Core.Entities.Dto;
using Core.Services;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class HesapTurGrupManager : IHesapTurGrupService
    {
        IEfHesapTurGrupData data;

        public HesapTurGrupManager(IEfHesapTurGrupData data)
        {
            this.data = data;
        }

        public EntityOperationResult<HesapTurGrup> Add(HesapTurGrup entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<HesapTurGrup> Update(HesapTurGrup entity)
        {
            throw new NotImplementedException();
        }

        EntityOperationResult<HesapTurGrup> IServiceModel<HesapTurGrup>.DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<HesapTurGrup, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public HesapTurGrup Get(Expression<Func<HesapTurGrup, bool>> expression = null, params Expression<Func<HesapTurGrup, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<HesapTurGrup> List(Expression<Func<HesapTurGrup, bool>> expression = null, params Expression<Func<HesapTurGrup, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<HesapTurGrup> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<HesapTurGrup>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
