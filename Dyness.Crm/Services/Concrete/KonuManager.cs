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
    public class KonuManager : IKonuService
    {
        IEfKonuData data;
        //IEfSinavSoruData sinavSoruData;

        public KonuManager(IEfKonuData data)//, IEfSinavSoruData sinavSoruData)
        {
            this.data = data;
            //this.sinavSoruData = sinavSoruData;
        }

        public EntityOperationResult<Konu> Add(Konu entity)
        {
            var entityOperationResult = new EntityOperationResult<Konu>(entity);

            var validationResults = EntityValidator<Konu>.ValidateEntity(entity).ToList();

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
                x.DersId == entity.DersId &&
                x.KurumId == entity.KurumId &&
                x.Kod.ToLower() == entity.Kod.ToLower());

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.VarolanKaydi
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
                Message = ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Konu> Update(Konu entity)
        {
            var entityOperationResult = new EntityOperationResult<Konu>(entity);

            var validationResults = EntityValidator<Konu>.ValidateEntity(entity).ToList();

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
                x.Kod.ToLower() == entity.Kod.ToLower() &&
                x.KonuId != entity.KonuId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.VarolanKaydi
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
                Message = ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Konu> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<Konu>();

            List<MessageInfo> validationResults = new List<MessageInfo>();

            //var checkExistsCount = sinavSoruData.GetByWhereCaseCount(x => x.KonuId == id);
            //
            //if (checkExistsCount > 0)
            //{
            //    entityOperationResult.MessageInfos.Add(new MessageInfo
            //    {
            //        MessageInfoType = MessageInfoType.Error,
            //        Message = ServiceNoticesResources.KonuSilebilmekIcin
            //    });
            //
            //    return entityOperationResult;
            //}

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

        public int GetCount(Expression<Func<Konu, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<Konu>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public Konu Get(Expression<Func<Konu, bool>> expression = null, params Expression<Func<Konu, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<Konu>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<Konu> List(Expression<Func<Konu, bool>> expression = null, params Expression<Func<Konu, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<Konu>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<Konu> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<Konu> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<Konu>
                {
                    data = data.GetAllIncludeMultiple(x => x.Ders)
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<Konu>
                {
                    data = data.GetByWhereCaseIncludeMultiple(x =>
                    x.KurumId == null ||
                    x.KurumId == Identity.KurumId,
                    y => y.Ders)
                };
            }

            return entityPagedDataSource;
        }
    }
}