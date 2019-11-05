using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entities.Concrete;
using Services.Abstract;
using Data.Abstract.EntityFramework;
using Core.Entities.Dto;
using System.Linq;
using Services.Validation;
using Core.Aspects.Postsharp.CacheAspects;

namespace Services.Concrete
{
    public class UlkeManager : IUlkeService
    {
        IEfUlkeData data;
        IEfSehirData sehirData;

        public UlkeManager(IEfUlkeData data, IEfSehirData sehirData)
        {
            this.data = data;
            this.sehirData = sehirData;
        }

        [CacheRemoveAspect]
        public EntityOperationResult<Ulke> Add(Ulke entity)
        {
            EntityOperationResult<Ulke> entityOperationResult = new EntityOperationResult<Ulke>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Ulke>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => x.UlkeAd.ToLower() == entity.UlkeAd.ToLower());

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
                Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        [CacheRemoveAspect]
        public EntityOperationResult<Ulke> Update(Ulke entity)
        {
            EntityOperationResult<Ulke> entityOperationResult = new EntityOperationResult<Ulke>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Ulke>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.UlkeAd.ToLower() == entity.UlkeAd.ToLower() &&
            x.UlkeId != entity.UlkeId);

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

        public EntityOperationResult<Ulke> DeleteById(int id)
        {
            EntityOperationResult<Ulke> entityOperationResult = new EntityOperationResult<Ulke>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = sehirData.GetByWhereCaseCount(x => x.UlkeId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.UlkeyiSilebilmekIcin
                });

                return entityOperationResult;
            }

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

        public int GetCount(Expression<Func<Ulke, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public Ulke Get(Expression<Func<Ulke, bool>> expression = null, params Expression<Func<Ulke, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        [CacheAspect]
        public IEnumerable<Ulke> List(Expression<Func<Ulke, bool>> expression = null, params Expression<Func<Ulke, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<Ulke> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<Ulke>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
