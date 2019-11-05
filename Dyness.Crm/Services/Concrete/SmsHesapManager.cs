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
    public class SmsHesapManager : ISmsHesapService
    {
        IEfSmsHesapData data;

        public SmsHesapManager(IEfSmsHesapData data)
        {
            this.data = data;
        }

        public EntityOperationResult<SmsHesap> Add(SmsHesap entity)
        {
            var entityOperationResult = new EntityOperationResult<SmsHesap>(entity);

            var validationResults = EntityValidator<SmsHesap>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.SubeId == Identity.SubeId &&
            x.Baslik.ToLower() == entity.Baslik.ToLower());

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            entity.OlusturulmaTarihi = DateTime.Now;
            entity.GuncellemeTarihi = DateTime.Now;
            entity.SonHareketTarihi = DateTime.Now;
            entity.SmsHesapDurumId = -1;

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

        public EntityOperationResult<SmsHesap> Update(SmsHesap entity)
        {
            var entityOperationResult = new EntityOperationResult<SmsHesap>(entity);

            var validationResults = EntityValidator<SmsHesap>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.SubeId == Identity.SubeId &&
            x.Baslik.ToLower() == entity.Baslik.ToLower() &&
            x.SmsHesapId != entity.SmsHesapId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            if (entity.SmsHesapDurumId == 0 || entity.SmsHesapDurumId == 1)
            {
                var oldModel = data.GetById(entity.SmsHesapId);

                if (!string.Equals(oldModel.Baslik.ToLower(), entity.Baslik.ToLower()))
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.SmsHesapBaslikDegistirilemez
                    });

                    return entityOperationResult;
                }
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
                Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<SmsHesap> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<SmsHesap>();

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
        public int GetCount(Expression<Func<SmsHesap, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<SmsHesap>();

                predicate = predicate.And(x => x.SubeId == Identity.SubeId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public SmsHesap Get(Expression<Func<SmsHesap, bool>> expression = null, params Expression<Func<SmsHesap, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SmsHesap>();

                predicate = predicate.And(x => x.SubeId == Identity.SubeId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<SmsHesap> List(Expression<Func<SmsHesap, bool>> expression = null, params Expression<Func<SmsHesap, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SmsHesap>();

                predicate = predicate.And(x => x.SubeId == Identity.SubeId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<SmsHesap> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<SmsHesap> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<SmsHesap>
                {
                    data = data.GetAllIncludeMultiple(x => x.SmsHesapDurum)
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<SmsHesap>
                {
                    data = data.GetByWhereCaseIncludeMultiple(x => x.SubeId == Identity.SubeId, y => y.SmsHesapDurum)
                };
            }

            return entityPagedDataSource;
        }
    }
}
