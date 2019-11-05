using Services.Abstract;
using System;
using System.Collections.Generic;
using Entities.Concrete;
using System.Linq.Expressions;
using Data.Abstract.EntityFramework;
using Core.Entities.Dto;
using System.Linq;
using Core.Aspects.Postsharp.CacheAspects;
using Services.Validation;

namespace Services.Concrete
{
    public class IlceManager : IIlceService
    {
        IEfIlceData data;

        public IlceManager(IEfIlceData data)
        {
            this.data = data;
        }

        [CacheRemoveAspect]
        public EntityOperationResult<Ilce> Add(Ilce entity)
        {
            EntityOperationResult<Ilce> entityOperationResult = new EntityOperationResult<Ilce>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Ilce>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.IlceAd.ToLower() == entity.IlceAd.ToLower() &&
            x.SehirId == entity.SehirId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            var returnMessage = data.Add(entity);

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
                Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        [CacheRemoveAspect]
        public EntityOperationResult<Ilce> Update(Ilce entity)
        {
            EntityOperationResult<Ilce> entityOperationResult = new EntityOperationResult<Ilce>
            {
                Model = entity
            };

            var validationResults = EntityValidator<Ilce>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.IlceAd.ToLower() == entity.IlceAd.ToLower() &&
            x.SehirId == entity.SehirId &&
            x.IlceId != entity.IlceId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

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

        public EntityOperationResult<Ilce> DeleteById(int id)
        {
            EntityOperationResult<Ilce> entityOperationResult = new EntityOperationResult<Ilce>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var returnMessage = data.DeleteById(id);

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
                Message = Core.Properties.ServiceNoticesResources.BasariylaSilindi
            });

            return entityOperationResult;
        }

        public int GetCount(Expression<Func<Ilce, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public Ilce Get(Expression<Func<Ilce, bool>> expression = null, params Expression<Func<Ilce, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        [CacheAspect]
        public IEnumerable<Ilce> List(Expression<Func<Ilce, bool>> expression = null, params Expression<Func<Ilce, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<Ilce> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<Ilce>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
