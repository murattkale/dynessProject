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
    public class HesapManager : IHesapService
    {
        IEfHesapData data;
        IEfHesapHareketData hesapHareketData;
        IDpHesapData dpData;
        IDpOgrenciHesapData dpOgrenciData;

        public HesapManager(
            IEfHesapData data, 
            IEfHesapHareketData hesapHareketData, 
            IDpHesapData dpData,
            IDpOgrenciHesapData dpOgrenciData)
        {
            this.data = data;
            this.hesapHareketData = hesapHareketData;
            this.dpData = dpData;
            this.dpOgrenciData = dpOgrenciData;
        }

        public EntityOperationResult<Hesap> Add(Hesap entity)
        {
            EntityOperationResult<Hesap> entityOperationResult = new EntityOperationResult<Hesap>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Hesap>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            entity.BagliKurumId = Identity.KurumId;

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.BagliKurumId == entity.BagliKurumId &&
                x.UstHesapId == entity.UstHesapId &&
                x.HesapBaslik.ToLower() == entity.HesapBaslik.ToLower() &&
                x.HesapTurId == entity.UstHesapId);

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

        public EntityOperationResult<Hesap> Update(Hesap entity)
        {
            EntityOperationResult<Hesap> entityOperationResult = new EntityOperationResult<Hesap>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()

            };

            var validationResults = EntityValidator<Hesap>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1 && entity.BagliKurumId != Identity.KurumId)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
               x.BagliKurumId == entity.BagliKurumId &&
               x.UstHesapId == entity.UstHesapId &&
               x.HesapBaslik.ToLower() == entity.HesapBaslik.ToLower() &&
               x.HesapId != entity.HesapId);

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

        public EntityOperationResult<Hesap> DeleteById(int id)
        {
            var entity = data.GetById(id);

            EntityOperationResult<Hesap> entityOperationResult = new EntityOperationResult<Hesap>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            if (entity.HesapTurId == 1 || entity.HesapTurId == 2 || entity.HesapTurId == 3)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.HesapSilinemezUyari
                });

                return entityOperationResult;
            }

            var hesapHareketAdet = hesapHareketData.GetByWhereCaseCount(x => x.AlacakliHesapId == id || x.BorcluHesapId == id);

            if(hesapHareketAdet > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.HesapHareketOlanHesapSilinemez
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
                Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public int GetCount(Expression<Func<Hesap, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public Hesap Get(Expression<Func<Hesap, bool>> expression = null, params Expression<Func<Hesap, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<Hesap>();

                predicate = predicate.And(x => x.BagliKurumId == null || x.BagliKurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<Hesap> List(Expression<Func<Hesap, bool>> expression = null, params Expression<Func<Hesap, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<Hesap>();

                predicate = predicate.And(x => x.BagliKurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<Hesap> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<HesapDto> HesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            return dpData.GetPagedEntities("Hesap_DigerHesap_Listele", filter, newParameters);
        }

        public EntityPagedDataSource<OgrenciHesapDto> OgrenciHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            return dpOgrenciData.GetPagedEntities("Hesap_Ogrenci_Listele", filter, newParameters);
        }

        public EntityPagedDataSource<HesapDto> PersonelHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            return dpData.GetPagedEntities("Hesap_Personel_Listele", filter, newParameters);
        }

        public EntityPagedDataSource<HesapDto> SubeHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            return dpData.GetPagedEntities("Hesap_Sube_Listele", filter, newParameters);
        }

        public EntityPagedDataSource<HesapDto> KasaBankaHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            return dpData.GetPagedEntities("Hesap_KasaBankaHesap_Listele", filter, newParameters);
        }

        public EntityPagedDataSource<HesapDto> GiderHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            return dpData.GetPagedEntities("Hesap_GiderHesap_Listele", filter, newParameters);
        }

        public EntityPagedDataSource<HesapDto> GelirHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));
            newParameters.Add(new Parameter("KurumId", Identity.KurumId));

            return dpData.GetPagedEntities("Hesap_GelirHesap_Listele", filter, newParameters);
        }

    }
}
