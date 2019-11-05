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
    public class KiyafetManager : IKiyafetService
    {
        IEfKiyafetData data;
        IEfOgrenciSozlesmeKiyafetDurumData ogrenciSozlesmeKiyafetDurumData;

        public KiyafetManager(IEfKiyafetData data, IEfOgrenciSozlesmeKiyafetDurumData ogrenciSozlesmeKiyafetDurumData)
        {
            this.data = data;
            this.ogrenciSozlesmeKiyafetDurumData = ogrenciSozlesmeKiyafetDurumData;
        }

        public EntityOperationResult<Kiyafet> Add(Kiyafet entity)
        {
            EntityOperationResult<Kiyafet> entityOperationResult = new EntityOperationResult<Kiyafet>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Kiyafet>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }
            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.KurumId == Identity.KurumId &&
                x.KiyafetAd.ToLower() == entity.KiyafetAd.ToLower() &&
                x.KiyafetTurId == entity.KiyafetTurId &&
                x.KiyafetBedenId == entity.KiyafetBedenId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            entity.KurumId = Identity.KurumId;

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

        public EntityOperationResult<Kiyafet> Update(Kiyafet entity)
        {
            EntityOperationResult<Kiyafet> entityOperationResult = new EntityOperationResult<Kiyafet>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Kiyafet>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1 && entity.KurumId != Identity.KurumId)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(
                x => x.KurumId == Identity.KurumId &&
                x.KiyafetAd.ToLower() == entity.KiyafetAd.ToLower() &&
                x.KiyafetTurId == entity.KiyafetTurId &&
                x.KiyafetBedenId == entity.KiyafetBedenId &&
                x.KiyafetId != entity.KiyafetId);

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

        public EntityOperationResult<Kiyafet> DeleteById(int id)
        {
            EntityOperationResult<Kiyafet> entityOperationResult = new EntityOperationResult<Kiyafet>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = ogrenciSozlesmeKiyafetDurumData.GetByWhereCaseCount(x => x.KiyafetId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.KiyafetSilebilmekIcin
                });

                return entityOperationResult;
            }

            var entity = data.GetById(id);

            if (Identity.KurumId != -1 && entity.KurumId != Identity.KurumId)
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

        public int GetCount(Expression<Func<Kiyafet, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<Kiyafet>();

                predicate = predicate.And(x => x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public Kiyafet Get(Expression<Func<Kiyafet, bool>> expression = null, params Expression<Func<Kiyafet, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<Kiyafet>();

                predicate = predicate.And(x =>x.KurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<Kiyafet> List(Expression<Func<Kiyafet, bool>> expression = null, params Expression<Func<Kiyafet, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<Kiyafet>();

                predicate = predicate.And(x => x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<Kiyafet> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<Kiyafet> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<Kiyafet>
                {
                    data = data.GetAllIncludeMultiple(
                     y => y.KiyafetTur,
                     y => y.KiyafetBeden)
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<Kiyafet>
                {
                    data = data.GetByWhereCaseIncludeMultiple(
                     x => x.KurumId == Identity.KurumId,
                     y => y.KiyafetTur,
                     y => y.KiyafetBeden)
                };
            }

            return entityPagedDataSource;
        }
    }
}
