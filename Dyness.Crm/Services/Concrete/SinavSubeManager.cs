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
    public class SinavSubeManager : ISinavSubeService
    {
        IEfSinavSubeData data;

        public SinavSubeManager(IEfSinavSubeData data)
        {
            this.data = data;
        }

        public EntityOperationResult<SinavSube> Add(SinavSube entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SinavSube> Update(SinavSube entity)
        {
            var entityOperationResult = new EntityOperationResult<SinavSube>(entity);

            var validationResults = EntityValidator<SinavSube>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var returnMessage = data.Update(entity);

            if (!string.IsNullOrEmpty(returnMessage))
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = returnMessage
                });

                return entityOperationResult;
            }

            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<SinavSube> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<SinavSube, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public SinavSube Get(Expression<Func<SinavSube, bool>> expression = null, params Expression<Func<SinavSube, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<SinavSube> List(Expression<Func<SinavSube, bool>> expression = null, params Expression<Func<SinavSube, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<SinavSube> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
