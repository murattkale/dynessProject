using Core.Entities.Dto;
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
    public class KullaniciManager : IKullaniciService
    {
        IEfKullaniciData data;

        public KullaniciManager(IEfKullaniciData data)
        {
            this.data = data;
        }

        public EntityOperationResult<Kullanici> Add(Kullanici entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<Kullanici> Update(Kullanici entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<Kullanici> Giris(string kullaniciAd, string sifre)
        {
            var entity = data.GetByWhereCaseIncludeMultiple(x => x.KullaniciAd == kullaniciAd && x.Sifre == sifre, y => y.Personel).FirstOrDefault();

            var entityOperationResult = new EntityOperationResult<Kullanici>(entity);

            if (entity == null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.KullanicBbulunamadi
                });

                return entityOperationResult;
            }

            if (!entity.EtkinMi)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.KullanıcıEtkinDegil
                });

                return entityOperationResult;
            }

            entity.SonGirisTarihi = DateTime.Now;
            entity.SifreTekrar = entity.Sifre;

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

            entityOperationResult.Model = Get(x =>
                x.PersonelId == entity.PersonelId,
            y => y.Personel.PersonelGrup,
            y => y.Personel.PersonelYetkiGrup,
            y => y.Personel.Ulke,
            y => y.Personel.Sube.Kurum,
            y => y.Personel.PersonelSubeYetkiler,
            y => y.Personel.PersonelYetkiGrup);

            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaGirisYapildi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Kullanici> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<Kullanici, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public Kullanici Get(Expression<Func<Kullanici, bool>> expression = null, params Expression<Func<Kullanici, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<Kullanici> List(Expression<Func<Kullanici, bool>> expression = null, params Expression<Func<Kullanici, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<Kullanici> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
