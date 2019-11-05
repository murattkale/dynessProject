﻿using Core.Entities.Dto;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class EhliyetTurManager : IEhliyetTurService
    {
        IEfEhliyetTurData data;

        public EhliyetTurManager(IEfEhliyetTurData data)
        {
            this.data = data;
        }

        public EntityOperationResult<EhliyetTur> Add(EhliyetTur entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<EhliyetTur> Update(EhliyetTur entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<EhliyetTur> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<EhliyetTur, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public EhliyetTur Get(Expression<Func<EhliyetTur, bool>> expression = null, params Expression<Func<EhliyetTur, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EhliyetTur> List(Expression<Func<EhliyetTur, bool>> expression = null, params Expression<Func<EhliyetTur, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<EhliyetTur> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}