using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Core.Services.Helpers;
using Data.Abstract.Dapper;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using Services.Validation;
using Services.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Services.Concrete
{
    public class PersonelManager : IPersonelService
    {
        IEfPersonelData data;
        IEfHesapData hesapData;
        IEfSubeData subeData;
        IDpPersonelData dpData;

        public PersonelManager(
            IEfPersonelData data,
            IEfHesapData hesapData,
            IEfSubeData subeData,
            IDpPersonelData dpData)
        {
            this.data = data;
            this.hesapData = hesapData;
            this.subeData = subeData;
            this.dpData = dpData;
        }

        public EntityOperationResult<Personel> Add(Personel entity)
        {
            EntityOperationResult<Personel> entityOperationResult = new EntityOperationResult<Personel>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Personel>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            // if (!string.IsNullOrEmpty(entity.TcKimlikNo))
            // {
            //     var checkTcKimlikNo = TcKimlikSorgulama.Dogrula(entity.TcKimlikNo, entity.Ad, entity.Soyad, entity.DogumTarihi.Value.Year);
            //
            //     if (!checkTcKimlikNo)
            //     {
            //         entityOperationResult.MessageInfos.Add(new MessageInfo
            //         {
            //             MessageInfoType = MessageInfoType.Error,
            //             Message = Core.Properties.ServiceNoticesResources.TcKimlikHatali
            //         });
            //
            //         return entityOperationResult;
            //     }
            //
            //     var checkExistsCount = data.GetByWhereCaseCount(x => x.TcKimlikNo == entity.TcKimlikNo && x.SubeId == entity.SubeId);
            //
            //     if (checkExistsCount > 0)
            //     {
            //         entityOperationResult.MessageInfos.Add(new MessageInfo
            //         {
            //             MessageInfoType = MessageInfoType.Error,
            //             Message = Core.Properties.ServiceNoticesResources.TcKimlikIleKayitliPersonelMevcut
            //         });
            //
            //         return entityOperationResult;
            //     }
            // }

            // Personel Etkin Değilse, İşten Ayrılma Tarihi Dolu Olmalı
            if (!entity.EtkinMi && entity.IstenAyrilmaTarihi == null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.EtkinDegilseIstenAyrilmaTarihiDoluOlmali
                });

                return entityOperationResult;
            }

            // Personelin şubelerden aldığı toplam ücret maaşından çok olamaz.
            var subeToplamUcret = entity.PersonelSubeUcretler.Count > 0
                ? entity.PersonelSubeUcretler.Sum(x => x.Ucret)
                : 0;

            if (entity.Maas < subeToplamUcret)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.PersonelinSubelerdenAldigiToplamMaasindanCokOlamaz
                });

                return entityOperationResult;
            }

            if (entity.Kullanici != null)
            {
                var checkExistsCount = data.GetByWhereCaseCount(x => x.Kullanici.KullaniciAd.ToLower() == entity.Kullanici.KullaniciAd.ToLower());

                if (checkExistsCount > 0)
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.KullaniciAdiKullanilmis
                    });

                    return entityOperationResult;
                }

                if (entity.PersonelGrupId == 0)
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.KullaniciPersonelGrupSecilmelidir
                    });

                    return entityOperationResult;
                }
            }

            var sube = subeData.GetById((int)entity.SubeId);

            entity.Hesap = new Hesap
            {
                BagliKurumId = sube.KurumId,
                UstHesapId = sube.SubeId,
                ParaBirimId = (int)sube.ParaBirimId,
                HesapBaslik = entity.AdSoyad,
                HesapTurId = 3,
                EtkinMi = true,
                Personel = entity
            };

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

        public EntityOperationResult<Personel> Update(Personel entity)
        {
            EntityOperationResult<Personel> entityOperationResult = new EntityOperationResult<Personel>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Personel>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);
            var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

            var subeEntity = subeData.GetById((int)entity.SubeId);

            if (Identity.KurumId != -1 &&
                subeEntity.KurumId != Identity.KurumId ||
                (subeEntity.KurumId == Identity.KurumId &&
                personelSubeYetkiler.Count > 0 &&
                !yetkiliSubeIdler.Contains((int)entity.SubeId)))
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            if (!string.IsNullOrEmpty(entity.TcKimlikNo))
            {
                var checkTcKimlikNo = TcKimlikSorgulama.Dogrula(entity.TcKimlikNo, entity.Ad, entity.Soyad, entity.DogumTarihi.Value.Year);

                if (!checkTcKimlikNo)
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.TcKimlikHatali
                    });

                    return entityOperationResult;
                }
            }

            // Personel Etkin Değilse, İşten Ayrılma Tarihi Dolu Olmalı
            if (!entity.EtkinMi && entity.IstenAyrilmaTarihi == null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.EtkinDegilseIstenAyrilmaTarihiDoluOlmali
                });

                return entityOperationResult;
            }

            // Personelin şubelerden aldığı toplam ücret maaşından çok olamaz.
            if (entity.PersonelSubeUcretler != null)
            {
                var subeToplamUcret = entity.PersonelSubeUcretler.Count > 0
                    ? entity.PersonelSubeUcretler.Where(x => !x.Silinecek).Sum(x => x.Ucret)
                    : 0;

                if (entity.Maas < subeToplamUcret)
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.PersonelinSubelerdenAldigiToplamMaasindanCokOlamaz
                    });

                    return entityOperationResult;
                }
            }

            if (entity.Kullanici != null)
            {
                var checkExistsCount = data.GetByWhereCaseCount(x => x.Kullanici.KullaniciAd.ToLower() == entity.Kullanici.KullaniciAd.ToLower() && x.Kullanici.PersonelId != entity.PersonelId);

                if (checkExistsCount > 0)
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.KullaniciAdiKullanilmis
                    });

                    return entityOperationResult;
                }

                if (entity.PersonelGrupId == 0)
                {
                    entityOperationResult.MessageInfos.Add(new MessageInfo
                    {
                        MessageInfoType = MessageInfoType.Error,
                        Message = Core.Properties.ServiceNoticesResources.KullaniciPersonelGrupSecilmelidir
                    });

                    return entityOperationResult;
                }
            }

            if (entity.PersonelSubeUcretler != null && entity.PersonelSubeUcretler.Count > 0)
            {
                entity.PersonelSubeUcretler = entity.PersonelSubeUcretler.OrderBy(x => x.Ucret).ToList();

                var personelSubeUcretlerCount = entity.PersonelSubeUcretler.Count;

                for (int i = 0; i < personelSubeUcretlerCount; i++)
                {
                    if (entity.PersonelSubeUcretler[0].Ucret != null && entity.PersonelSubeUcretler[0].Ucret > 0)
                        break;

                    entity.PersonelSubeUcretler.RemoveAt(0);
                }
            }

            entity.Hesap = hesapData.GetById(entity.PersonelId);
            entity.Hesap.HesapBaslik = entity.AdSoyad;

            //Kullanicisi olmayabilir
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

        public EntityOperationResult<Personel> DeleteById(int id)
        {
            var newParameters = new List<Parameter>()
            {
                new Parameter("PersonelId", id)
            };

            var entityOperationResult = new EntityOperationResult<Personel>(null);

            var returnCount = dpData.ExecuteGetRowCount("Personel_Sil", newParameters);

            if (returnCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error
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

        public int GetCount(Expression<Func<Personel, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    var predicate = PredicateBuilder.True<Personel>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Personel>();

                    predicate = predicate.And(x =>
                        x.Sube.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Count(y => y == x.SubeId) > 0).
                    And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public Personel Get(Expression<Func<Personel, bool>> expression = null, params Expression<Func<Personel, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    var predicate = PredicateBuilder.True<Personel>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Personel>();

                    predicate = predicate.And(x =>
                        x.Sube.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Count(y => y == x.SubeId) > 0).
                    And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<Personel> List(Expression<Func<Personel, bool>> expression = null, params Expression<Func<Personel, object>>[] includes)
        {
            var newIncludes = new Expression<Func<Personel, object>>[includes == null ? 0 : includes.Length + 1];

            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    newIncludes[i] = includes[i];
                }
            }

            newIncludes[newIncludes.Length - 1] = y => y.PersonelGrup;

            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, newIncludes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    var predicate = PredicateBuilder.True<Personel>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, newIncludes);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Personel>();

                    predicate = predicate.And(x =>
                        x.Sube.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Count(y => y == x.SubeId) > 0).
                    And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, newIncludes);
                }
            }
        }

        public EntityPagedDataSource<Personel> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<Personel> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<Personel>
                {
                    data = data.GetAllIncludeMultiple(
                        y => y.PersonelGrup,
                        y => y.PersonelYetkiGrup,
                        y => y.Sube.ParaBirim,
                        y => y.Sube.Kurum,
                        y => y.Kullanici)
                };
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    entityPagedDataSource = new EntityPagedDataSource<Personel>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x =>
                            x.Sube.KurumId == Identity.KurumId,
                            y => y.PersonelGrup,
                            y => y.PersonelYetkiGrup,
                            y => y.Sube.ParaBirim,
                            y => y.Sube.Kurum,
                            y => y.Kullanici)
                    };
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    entityPagedDataSource = new EntityPagedDataSource<Personel>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(
                            x => yetkiliSubeIdler.Count(y => y == x.SubeId) > 0,
                        y => y.PersonelGrup,
                        y => y.PersonelYetkiGrup,
                        y => y.Sube.ParaBirim,
                        y => y.Sube.Kurum,
                        y => y.Kullanici)
                    };
                }
            }

            return entityPagedDataSource;
        }
    }
}
