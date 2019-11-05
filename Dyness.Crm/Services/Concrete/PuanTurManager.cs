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
    public class PuanTurManager : IPuanTurService
    {
        IEfPuanTurData data;

        public PuanTurManager(IEfPuanTurData data)
        {
            this.data = data;
        }

        public EntityOperationResult<PuanTur> Add(PuanTur entity)
        {
            var entityOperationResult = new EntityOperationResult<PuanTur>(entity);

            var validationResults = EntityValidator<PuanTur>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1)
                entity.KurumId = Identity.KurumId;

            var checkExistsCount = data.GetByWhereCaseCount(x =>
               x.KurumId == Identity.KurumId &&
               x.PuanTurAd == entity.PuanTurAd);

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

        public EntityOperationResult<PuanTur> Update(PuanTur entity)
        {
            var entityOperationResult = new EntityOperationResult<PuanTur>(entity);

            var validationResults = EntityValidator<PuanTur>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1 &&
                entity.KurumId != Identity.KurumId ||
                entity.KurumId == -1)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => x.PuanTurAd.ToLower() == entity.PuanTurAd.ToLower() && x.PuanTurId != entity.PuanTurId);

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

        public EntityOperationResult<PuanTur> DeleteById(int id)
        {
            EntityOperationResult<PuanTur> entityOperationResult = new EntityOperationResult<PuanTur>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var entity = data.GetById(id);

            if (Identity.KurumId != -1 &&
                entity.KurumId != Identity.KurumId ||
                entity.KurumId == -1)
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

        public int GetCount(Expression<Func<PuanTur, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<PuanTur>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public PuanTur Get(Expression<Func<PuanTur, bool>> expression = null, params Expression<Func<PuanTur, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<PuanTur>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<PuanTur> List(Expression<Func<PuanTur, bool>> expression = null, params Expression<Func<PuanTur, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<PuanTur>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<PuanTur> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<PuanTur> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<PuanTur>
                {
                    data = data.GetAll()
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<PuanTur>
                {
                    data = data.GetByWhereCase(x =>
                    x.KurumId == null ||
                    x.KurumId == Identity.KurumId)
                };
            }

            return entityPagedDataSource;
        }
    }
}
