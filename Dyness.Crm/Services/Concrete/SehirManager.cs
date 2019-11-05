using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Core.Entities.Dto;
using System.Linq;
using Services.Validation;
using Core.Aspects.Postsharp.CacheAspects;

namespace Services.Concrete
{
    public class SehirManager : ISehirService
    {
        IEfSehirData data;
        IEfSubeData subeData;
        IEfIlceData ilceData;

        public SehirManager(IEfSehirData data, IEfSubeData subeData, IEfIlceData ilceData)
        {
            this.data = data;
            this.subeData = subeData;
            this.ilceData = ilceData;
        }

        [CacheRemoveAspect]
        public EntityOperationResult<Sehir> Add(Sehir entity)
        {
            EntityOperationResult<Sehir> entityOperationResult = new EntityOperationResult<Sehir>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Sehir>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.SehirAd.ToLower() == entity.SehirAd.ToLower() &&
            x.UlkeId == entity.UlkeId);

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
        public EntityOperationResult<Sehir> Update(Sehir entity)
        {
            EntityOperationResult<Sehir> entityOperationResult = new EntityOperationResult<Sehir>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Sehir>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.SehirAd.ToLower() == entity.SehirAd.ToLower() &&
            x.UlkeId == entity.UlkeId &&
            x.SehirId != entity.SehirId);

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

        public EntityOperationResult<Sehir> DeleteById(int id)
        {
            EntityOperationResult<Sehir> entityOperationResult = new EntityOperationResult<Sehir>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = subeData.GetByWhereCaseCount(x => x.SehirId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SehriSilebilmekIcinSubeler
                });

                return entityOperationResult;
            }

            checkExistsCount = ilceData.GetByWhereCaseCount(x => x.SehirId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SehriSilebilmekIcinIlceler
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

        public int GetCount(Expression<Func<Sehir, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public Sehir Get(Expression<Func<Sehir, bool>> expression = null, params Expression<Func<Sehir, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        [CacheAspect]
        public IEnumerable<Sehir> List(Expression<Func<Sehir, bool>> expression = null, params Expression<Func<Sehir, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<Sehir> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<Sehir>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
