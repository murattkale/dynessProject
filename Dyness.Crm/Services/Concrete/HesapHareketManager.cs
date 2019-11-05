using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.Services.Helpers;
using Data.Abstract.Dapper;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using Services.Validation;

namespace Services.Concrete
{
    public class HesapHareketManager : IHesapHareketService
    {
        readonly IEfHesapHareketData data;
        readonly IDpHesapHareketData dpData;

        public HesapHareketManager(IEfHesapHareketData data, IDpHesapHareketData dpData)
        {
            this.data = data;
            this.dpData = dpData;
        }

        public EntityOperationResult<HesapHareket> Add(HesapHareket entity)
        {
           var entityOperationResult = new EntityOperationResult<HesapHareket>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<HesapHareket>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

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

            entity = Get(x => x.HesapHareketId == entity.HesapHareketId, y => y.BorcluHesap, y => y.AlacakliHesap);

            entityOperationResult.Model = entity;
            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<HesapHareket> AddUpdateLists(OgrenciSozlesme ogrenciSozlesme, List<HesapHareket> eklenecekler, List<HesapHareket> guncellenecekler)
        {
            var entityOperationResult = new EntityOperationResult<HesapHareket>
            {
                Model = null,
                MessageInfos = new List<MessageInfo>()
            };

            var returnMessage = data.AddUpdateLists(ogrenciSozlesme, eklenecekler, guncellenecekler);

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

        public EntityOperationResult<HesapHareket> Update(HesapHareket entity)
        {
            EntityOperationResult<HesapHareket> entityOperationResult = new EntityOperationResult<HesapHareket>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<HesapHareket>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (entity.AlacakliHesapId == null || entity.AlacakliHesapId == 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OdenecekHesapSeciliOlmali
                });

                return entityOperationResult;
            }

            if (entity.BorcluHesapId == null ||entity.BorcluHesapId == 0 )
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OdemeHesapSecilmeli
                });

                return entityOperationResult;
            }

            if (entity.HareketTarihi == null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OdemeHareketTarihi
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

        public EntityOperationResult<HesapHareket> UpdateWithOgrenciSozlesme(HesapHareket entity, OgrenciSozlesme ogrenciSozlesme)
        {
            var entityOperationResult = new EntityOperationResult<HesapHareket>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<HesapHareket>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (entity.AlacakliHesapId == null || entity.AlacakliHesapId == 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OdenecekHesapSeciliOlmali
                });

                return entityOperationResult;
            }

            if (entity.BorcluHesapId == null || entity.BorcluHesapId == 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OdemeHesapSecilmeli
                });

                return entityOperationResult;
            }

            if (entity.HareketTarihi == null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OdemeHareketTarihi
                });

                return entityOperationResult;
            }

            var returnMessage = data.UpdateWithOgrenciSozlesme(entity, ogrenciSozlesme);

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

        public EntityOperationResult<HesapHareket> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<HesapHareket>();

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var entity = data.GetByWhereCaseIncludeMultiple(x =>
                x.HesapHareketId == id,
                y => y.AlacakliHesap,
                y => y.BorcluHesap).FirstOrDefault();

            if (Identity.KurumId != -1 && (entity.AlacakliHesap.BagliKurumId != Identity.KurumId || entity.BorcluHesap.BagliKurumId != Identity.KurumId))
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

        public int GetCount(Expression<Func<HesapHareket, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public int GetMinimumYear()
        {
            Expression<Func<HesapHareket, bool>> expression = x => x.IslemGerceklestiMi && x.HareketTarihi != null;
            Expression<Func<HesapHareket, DateTime?>> orderByExpression = x => x.HareketTarihi;

            return data.GetByWhereCaseByOrderByTakeIncludeMultiple(expression, orderByExpression, 1, false).FirstOrDefault().HareketTarihi.Value.Year;
        }

        public int GetMaximumYear()
        {
            Expression<Func<HesapHareket, bool>> expression = x => x.IslemGerceklestiMi && x.HareketTarihi != null;
            Expression<Func<HesapHareket, DateTime?>> orderByExpression = x => x.HareketTarihi;

            return data.GetByWhereCaseByOrderByTakeIncludeMultiple(expression, orderByExpression, 1, true).FirstOrDefault().HareketTarihi.Value.Year;
        }

        public HesapHareket Get(Expression<Func<HesapHareket, bool>> expression = null, params Expression<Func<HesapHareket, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<HesapHareket>();

                predicate = predicate.And(x => x.AlacakliHesap.BagliKurumId == Identity.KurumId || x.BorcluHesap.BagliKurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<HesapHareket> List(Expression<Func<HesapHareket, bool>> expression = null, params Expression<Func<HesapHareket, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<HesapHareket>();

                predicate = predicate.And(x => x.AlacakliHesap.BagliKurumId == Identity.KurumId || x.BorcluHesap.BagliKurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<HesapHareket> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<HesapHareketDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            return dpData.GetPagedEntities("HesapHareket_Listele", filter, newParameters);
        }
    }
}
