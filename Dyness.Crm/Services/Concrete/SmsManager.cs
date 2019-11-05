using Core.Entities.Dto;
using Core.Properties;
using Data.Abstract.Dapper;
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
    public class SmsManager : ISmsService
    {
        IEfSmsData data;
        IDpOgrenciTelefonData dpOgrenciTelefonData;
        IDpPersonelTelefonData dpPersonelTelefonData;

        public SmsManager(
            IEfSmsData data,
            IDpOgrenciTelefonData dpOgrenciTelefonData,
            IDpPersonelTelefonData dpPersonelTelefonData)
        {
            this.data = data;
            this.dpOgrenciTelefonData = dpOgrenciTelefonData;
            this.dpPersonelTelefonData = dpPersonelTelefonData;
        }

        public EntityOperationResult<Sms> Add(Sms entity)
        {
            var entityOperationResult = new EntityOperationResult<Sms>(entity);

            var validationResults = EntityValidator<Sms>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var returnMessage = data.AddWithSmsHesapHareket(entity);

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
                Message = ServiceNoticesResources.BasariylaGonderildi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Sms> Update(Sms entity)
        {
            var entityOperationResult = new EntityOperationResult<Sms>(entity);

            var validationResults = EntityValidator<Sms>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

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

        public EntityOperationResult<Sms> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<Sms, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public Sms Get(Expression<Func<Sms, bool>> expression = null, params Expression<Func<Sms, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Sms> List(Expression<Func<Sms, bool>> expression = null, params Expression<Func<Sms, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SmsTelefonBilgiDto> SmsTelefonBilgiListele(int tip, IEnumerable<Parameter> parameters)
        {
            var telefonlar = new List<SmsTelefonBilgiDto>();

            if (tip == 0)
            {
                var ogrenciTelefonlar = dpOgrenciTelefonData.GetEntities("Ogrenci_Telefon_Listele", parameters);

                if (ogrenciTelefonlar != null && ogrenciTelefonlar.Any())
                {
                    foreach (var ogrenciTelefon in ogrenciTelefonlar)
                    {
                        var smsTelefonBilgiDto = new SmsTelefonBilgiDto
                        {
                            Id = ogrenciTelefon.OgrenciId,
                            SubeId = ogrenciTelefon.SubeId,
                            SezonId = ogrenciTelefon.SezonId,
                            SinifId = ogrenciTelefon.SinifId,
                            AdSoyad = ogrenciTelefon.OgrenciAdSoyad,
                            SubeAd = ogrenciTelefon.SubeAd,
                            SezonAd = ogrenciTelefon.SezonAd,
                            SinifAd = ogrenciTelefon.SinifAd,
                            Telefon = ogrenciTelefon.OgrenciCepTelefon,
                            Tip = Tip.Ogrenci,
                            Bilgi = Bilgi.Kendisi,
                            Iletisim = ogrenciTelefon.IletisimKendi,
                            Checked = true
                        };

                        telefonlar.Add(smsTelefonBilgiDto);

                        #region Anne Telefonlar

                        if (!string.IsNullOrEmpty(ogrenciTelefon.AnneCepTelefon1))
                        {
                            var anneSmsTelefonBilgiDto = new SmsTelefonBilgiDto
                            {
                                Id = ogrenciTelefon.OgrenciId,
                                SubeId = ogrenciTelefon.SubeId,
                                SezonId = ogrenciTelefon.SezonId,
                                SinifId = ogrenciTelefon.SinifId,
                                AdSoyad = ogrenciTelefon.AnneAdSoyad,
                                SubeAd = ogrenciTelefon.SubeAd,
                                SezonAd = ogrenciTelefon.SezonAd,
                                SinifAd = ogrenciTelefon.SinifAd,
                                Telefon = ogrenciTelefon.AnneCepTelefon1,
                                Tip = Tip.Ogrenci,
                                Bilgi = Bilgi.Anne,
                                Iletisim = ogrenciTelefon.IletisimAnne,
                                Checked = true
                            };

                            telefonlar.Add(anneSmsTelefonBilgiDto);
                        }

                        if (!string.IsNullOrEmpty(ogrenciTelefon.AnneCepTelefon2))
                        {
                            var anneSmsTelefonBilgiDto = new SmsTelefonBilgiDto
                            {
                                Id = ogrenciTelefon.OgrenciId,
                                SubeId = ogrenciTelefon.SubeId,
                                SezonId = ogrenciTelefon.SezonId,
                                SinifId = ogrenciTelefon.SinifId,
                                AdSoyad = ogrenciTelefon.AnneAdSoyad,
                                SubeAd = ogrenciTelefon.SubeAd,
                                SezonAd = ogrenciTelefon.SezonAd,
                                SinifAd = ogrenciTelefon.SinifAd,
                                Telefon = ogrenciTelefon.AnneCepTelefon2,
                                Tip = Tip.Ogrenci,
                                Bilgi = Bilgi.Anne,
                                Iletisim = ogrenciTelefon.IletisimAnne,
                                Checked = true
                            };

                            telefonlar.Add(anneSmsTelefonBilgiDto);
                        }

                        #endregion

                        #region Baba Telefonlar

                        if (!string.IsNullOrEmpty(ogrenciTelefon.BabaCepTelefon1))
                        {
                            var babaSmsTelefonBilgiDto = new SmsTelefonBilgiDto
                            {
                                Id = ogrenciTelefon.OgrenciId,
                                SubeId = ogrenciTelefon.SubeId,
                                SezonId = ogrenciTelefon.SezonId,
                                SinifId = ogrenciTelefon.SinifId,
                                AdSoyad = ogrenciTelefon.BabaAdSoyad,
                                SubeAd = ogrenciTelefon.SubeAd,
                                SezonAd = ogrenciTelefon.SezonAd,
                                SinifAd = ogrenciTelefon.SinifAd,
                                Telefon = ogrenciTelefon.BabaCepTelefon1,
                                Tip = Tip.Ogrenci,
                                Bilgi = Bilgi.Baba,
                                Iletisim = ogrenciTelefon.IletisimBaba,
                                Checked = true
                            };

                            telefonlar.Add(babaSmsTelefonBilgiDto);
                        }

                        if (!string.IsNullOrEmpty(ogrenciTelefon.BabaCepTelefon2))
                        {
                            var babaSmsTelefonBilgiDto = new SmsTelefonBilgiDto
                            {
                                Id = ogrenciTelefon.OgrenciId,
                                SubeId = ogrenciTelefon.SubeId,
                                SezonId = ogrenciTelefon.SezonId,
                                SinifId = ogrenciTelefon.SinifId,
                                AdSoyad = ogrenciTelefon.BabaAdSoyad,
                                SubeAd = ogrenciTelefon.SubeAd,
                                SezonAd = ogrenciTelefon.SezonAd,
                                SinifAd = ogrenciTelefon.SinifAd,
                                Telefon = ogrenciTelefon.BabaCepTelefon2,
                                Tip = Tip.Ogrenci,
                                Bilgi = Bilgi.Baba,
                                Iletisim = ogrenciTelefon.IletisimBaba,
                                Checked = true
                            };

                            telefonlar.Add(babaSmsTelefonBilgiDto);
                        }

                        #endregion

                        #region Yakını Telefonlar

                        if (!string.IsNullOrEmpty(ogrenciTelefon.YakiniCepTelefon1))
                        {
                            var yakiniSmsTelefonBilgiDto = new SmsTelefonBilgiDto
                            {
                                Id = ogrenciTelefon.OgrenciId,
                                SubeId = ogrenciTelefon.SubeId,
                                SezonId = ogrenciTelefon.SezonId,
                                SinifId = ogrenciTelefon.SinifId,
                                AdSoyad = ogrenciTelefon.YakiniAdSoyad,
                                SubeAd = ogrenciTelefon.SubeAd,
                                SezonAd = ogrenciTelefon.SezonAd,
                                SinifAd = ogrenciTelefon.SinifAd,
                                Telefon = ogrenciTelefon.YakiniCepTelefon1,
                                Tip = Tip.Ogrenci,
                                Bilgi = Bilgi.Yakini,
                                Iletisim = ogrenciTelefon.IletisimYakini,
                                Checked = true
                            };

                            telefonlar.Add(yakiniSmsTelefonBilgiDto);
                        }

                        if (!string.IsNullOrEmpty(ogrenciTelefon.YakiniCepTelefon2))
                        {
                            var yakiniSmsTelefonBilgiDto = new SmsTelefonBilgiDto
                            {
                                Id = ogrenciTelefon.OgrenciId,
                                SubeId = ogrenciTelefon.SubeId,
                                SezonId = ogrenciTelefon.SezonId,
                                SinifId = ogrenciTelefon.SinifId,
                                AdSoyad = ogrenciTelefon.YakiniAdSoyad,
                                SubeAd = ogrenciTelefon.SubeAd,
                                SezonAd = ogrenciTelefon.SezonAd,
                                SinifAd = ogrenciTelefon.SinifAd,
                                Telefon = ogrenciTelefon.YakiniCepTelefon2,
                                Tip = Tip.Ogrenci,
                                Bilgi = Bilgi.Yakini,
                                Iletisim = ogrenciTelefon.IletisimYakini,
                                Checked = true
                            };

                            telefonlar.Add(yakiniSmsTelefonBilgiDto);
                        }

                        #endregion
                    }
                }
            }
            else
            {
                var personelTelefonlar = dpPersonelTelefonData.GetEntities("Personel_Telefon_Listele", parameters);

                if (personelTelefonlar != null && personelTelefonlar.Any())
                {
                    foreach (var personelTelefon in personelTelefonlar)
                    {
                        var smsTelefonBilgiDto = new SmsTelefonBilgiDto
                        {
                            Id = personelTelefon.PersonelId,
                            SubeId = personelTelefon.SubeId,
                            PersonelGrupId = personelTelefon.PersonelGrupId,
                            AdSoyad = personelTelefon.PersonelAdSoyad,
                            SubeAd = personelTelefon.SubeAd,
                            PersonelGrupAd = personelTelefon.PersonelGrupAd,
                            Telefon = personelTelefon.Telefon,
                            Tip = Tip.Personel,
                            Bilgi = Bilgi.Kendisi,
                            Iletisim = true,
                            Checked = true
                        };

                        telefonlar.Add(smsTelefonBilgiDto);
                    }
                }
            }

            return telefonlar;
        }

        public EntityPagedDataSource<Sms> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
