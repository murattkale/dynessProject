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
    public class SinifSeviyeManager : ISinifSeviyeService
    {
        IEfSinifSeviyeData data;
        IEfSinifData sinifData;

        public SinifSeviyeManager(IEfSinifSeviyeData data, IEfSinifData sinifData)
        {
            this.data = data;
            this.sinifData = sinifData;
        }

        public EntityOperationResult<SinifSeviye> Add(SinifSeviye entity)
        {
            EntityOperationResult<SinifSeviye> entityOperationResult = new EntityOperationResult<SinifSeviye>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<SinifSeviye>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1)
            {
                entity.KurumId = Identity.KurumId;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.KurumId == entity.KurumId &&
                x.SinifSeviyeAd.ToLower() == entity.SinifSeviyeAd.ToLower() &&
                x.SinifSeviyeId == entity.SinifSeviyeId);

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

        public EntityOperationResult<SinifSeviye> Update(SinifSeviye entity)
        {
            EntityOperationResult<SinifSeviye> entityOperationResult = new EntityOperationResult<SinifSeviye>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<SinifSeviye>.ValidateEntity(entity).ToList();

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

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.KurumId == entity.KurumId &&
                x.SinifSeviyeAd.ToLower() == entity.SinifSeviyeAd.ToLower() &&
                x.SinifSeviyeId != entity.SinifSeviyeId);

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

        public EntityOperationResult<SinifSeviye> DeleteById(int id)
        {
            EntityOperationResult<SinifSeviye> entityOperationResult = new EntityOperationResult<SinifSeviye>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = sinifData.GetByWhereCaseCount(x => x.SinifSeviyeId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SinifSeviyeSilebilmekIcin
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

        public int GetCount(Expression<Func<SinifSeviye, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinifSeviye>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public SinifSeviye Get(Expression<Func<SinifSeviye, bool>> expression = null, params Expression<Func<SinifSeviye, object>>[] includes)
        {

            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinifSeviye>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<SinifSeviye> List(Expression<Func<SinifSeviye, bool>> expression = null, params Expression<Func<SinifSeviye, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinifSeviye>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<SinifSeviye> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<SinifSeviye> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<SinifSeviye>
                {
                    data = data.GetAll()
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<SinifSeviye>
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
