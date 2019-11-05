using Services.Abstract;
using System;
using System.Collections.Generic;
using Entities.Concrete;
using System.Linq.Expressions;
using Core.Entities.Dto;
using System.Linq;
using Data.Abstract.EntityFramework;
using Services.Validation;

namespace Services.Concrete
{
    public class ParaBirimManager : IParaBirimService
    {
        IEfParaBirimData data;
        IEfSubeData subeData;

        public ParaBirimManager(IEfParaBirimData data, IEfSubeData subeData)
        {
            this.data = data;
            this.subeData = subeData;
        }

        public EntityOperationResult<ParaBirim> Add(ParaBirim entity)
        {
            EntityOperationResult<ParaBirim> entityOperationResult = new EntityOperationResult<ParaBirim>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<ParaBirim>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => x.ParaBirimAd.ToLower() == entity.ParaBirimAd.ToLower());

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

        public EntityOperationResult<ParaBirim> Update(ParaBirim entity)
        {
            EntityOperationResult<ParaBirim> entityOperationResult = new EntityOperationResult<ParaBirim>
            {
                Model = entity
            };

            var validationResults = EntityValidator<ParaBirim>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.ParaBirimAd.ToLower() == entity.ParaBirimAd.ToLower() &&
            x.ParaBirimId != entity.ParaBirimId);

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

        public EntityOperationResult<ParaBirim> DeleteById(int id)
        {
            EntityOperationResult<ParaBirim> entityOperationResult = new EntityOperationResult<ParaBirim>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = subeData.GetByWhereCaseCount(x => x.ParaBirimId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.ParaBirimiSilebilmekIcin
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

        public int GetCount(Expression<Func<ParaBirim, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public ParaBirim Get(Expression<Func<ParaBirim, bool>> expression = null, params Expression<Func<ParaBirim, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<ParaBirim> List(Expression<Func<ParaBirim, bool>> expression = null, params Expression<Func<ParaBirim, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<ParaBirim> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<ParaBirim>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
