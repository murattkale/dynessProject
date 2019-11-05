using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
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
    public class SmsMetinSablonManager : ISmsMetinSablonService
    {
        IEfSmsMetinSablonData data;

        public SmsMetinSablonManager(IEfSmsMetinSablonData data)
        {
            this.data = data;
        }

        public EntityOperationResult<SmsMetinSablon> Add(SmsMetinSablon entity)
        {
            var entityOperationResult = new EntityOperationResult<SmsMetinSablon>(entity);

            var validationResults = EntityValidator<SmsMetinSablon>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1)
            {
                entity.SubeId = Identity.SubeId;
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

        public EntityOperationResult<SmsMetinSablon> Update(SmsMetinSablon entity)
        {
            var entityOperationResult = new EntityOperationResult<SmsMetinSablon>(entity);

            var validationResults = EntityValidator<SmsMetinSablon>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1 &&
               entity.SubeId != Identity.SubeId ||
               entity.SubeId == -1)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
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

        public EntityOperationResult<SmsMetinSablon> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<SmsMetinSablon>();

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var entity = data.GetById(id);

            if (Identity.KurumId != -1 &&
                 entity.SubeId != Identity.SubeId ||
                 entity.SubeId == -1)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
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

        public int GetCount(Expression<Func<SmsMetinSablon, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<SmsMetinSablon>();

                predicate = predicate.And(x => x.SubeId == null || x.SubeId == Identity.SubeId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public SmsMetinSablon Get(Expression<Func<SmsMetinSablon, bool>> expression = null, params Expression<Func<SmsMetinSablon, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SmsMetinSablon>();

                predicate = predicate.And(x => x.SubeId == null || x.SubeId == Identity.SubeId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<SmsMetinSablon> List(Expression<Func<SmsMetinSablon, bool>> expression = null, params Expression<Func<SmsMetinSablon, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SmsMetinSablon>();

                predicate = predicate.And(x => x.SubeId == null || x.SubeId == Identity.SubeId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<SmsMetinSablon> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<SmsMetinSablon> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<SmsMetinSablon>
                {
                    data = data.GetAll()
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<SmsMetinSablon>
                {
                    data = data.GetByWhereCase(x =>
                    x.SubeId == null ||
                    x.SubeId == Identity.SubeId)
                };
            }

            return entityPagedDataSource;
        }
    }
}
