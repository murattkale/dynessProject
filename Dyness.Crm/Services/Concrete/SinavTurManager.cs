using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.Properties;
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
    public class SinavTurManager : ISinavTurService
    {
        IEfSinavTurData data;
        IEfSinavData sinavData;

        public SinavTurManager(IEfSinavTurData data, IEfSinavData sinavData)
        {
            this.data = data;
            this.sinavData = sinavData;
        }

        public EntityOperationResult<SinavTur> Add(SinavTur entity)
        {
            var entityOperationResult = new EntityOperationResult<SinavTur>(entity);

            var validationResults = EntityValidator<SinavTur>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1)
                entity.KurumId = Identity.KurumId;

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.KurumId == entity.KurumId &&
                x.SinavTurAd.ToLower() == entity.SinavTurAd.ToLower());

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            var returnMessage = data.AddWithNestedLists(entity);

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
                Message = ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<SinavTur> Update(SinavTur entity)
        {
            var entityOperationResult = new EntityOperationResult<SinavTur>(entity);

            var validationResults = EntityValidator<SinavTur>.ValidateEntity(entity).ToList();

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
                    Message = ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.KurumId == entity.KurumId &&
                x.SinavTurAd.ToLower() == entity.SinavTurAd.ToLower() &&
                x.SinavTurId != entity.SinavTurId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            var returnMessage = data.UpdateWithNestedLists(entity);

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
                Message = ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<SinavTur> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<SinavTur>();

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = sinavData.GetByWhereCaseCount(x => x.SinavTurId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.SinavTurSilebilmekIcin
                });

                return entityOperationResult;
            }

            var entity = data.GetById(id);

            if (Identity.KurumId != -1 &&
                entity.KurumId != Identity.KurumId ||
                entity.KurumId == -1)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.YetkisizIslem
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
                Message = ServiceNoticesResources.BasariylaSilindi
            });

            return entityOperationResult;
        }

        public int GetCount(Expression<Func<SinavTur, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinavTur>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public SinavTur Get(Expression<Func<SinavTur, bool>> expression = null, params Expression<Func<SinavTur, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinavTur>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<SinavTur> List(Expression<Func<SinavTur, bool>> expression = null, params Expression<Func<SinavTur, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinavTur>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<SinavTur> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<SinavTur> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<SinavTur>
                {
                    data = data.GetAll()
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<SinavTur>
                {
                    data = data.GetByWhereCase(x => x.KurumId == null || x.KurumId == Identity.KurumId)
                };
            }

            return entityPagedDataSource;
        }
    }
}
