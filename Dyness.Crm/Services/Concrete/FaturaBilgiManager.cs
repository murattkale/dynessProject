using Core.Entities.Dto;
using Core.Services;
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
    public class FaturaBilgiManager : IFaturaBilgiService
    {
        IEfFaturaBilgiData data;
        IEfOgrenciSozlesmeData dataOgrenciSozlesme;

        public FaturaBilgiManager(IEfFaturaBilgiData data, IEfOgrenciSozlesmeData dataOgrenciSozlesme)
        {
            this.data = data;
            this.dataOgrenciSozlesme = dataOgrenciSozlesme;
        }

        public EntityOperationResult<FaturaBilgi> Add(FaturaBilgi entity)
        {
            var entityOperationResult = new EntityOperationResult<FaturaBilgi>(entity);

            var validationResults = EntityValidator<FaturaBilgi>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            string returnMessage = data.Add(entity);

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

        public EntityOperationResult<FaturaBilgi> Update(FaturaBilgi entity)
        {
            var entityOperationResult = new EntityOperationResult<FaturaBilgi>(entity);

            var validationResults = EntityValidator<FaturaBilgi>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            string returnMessage =  data.Update(entity);

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

        EntityOperationResult<FaturaBilgi> IServiceModel<FaturaBilgi>.DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<FaturaBilgi, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public FaturaBilgi Get(Expression<Func<FaturaBilgi, bool>> expression = null, params Expression<Func<FaturaBilgi, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<FaturaBilgi> List(Expression<Func<FaturaBilgi, bool>> expression = null, params Expression<Func<FaturaBilgi, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        EntityPagedDataSource<FaturaBilgi> IServiceModel<FaturaBilgi>.EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
