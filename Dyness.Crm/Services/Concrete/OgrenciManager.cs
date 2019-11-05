using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Core.Services.Helpers;
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
    public class OgrenciManager : IOgrenciService
    {
        IEfOgrenciData data;
        IEfHesapData hesapData;
        IEfSubeData subeData;
        IEfSezonData sezonData;
        IDpOgrenciData dpData;

        public OgrenciManager(
            IEfOgrenciData data,
            IEfOgrenciSozlesmeData sozlesmeData,
            IEfHesapData hesapData,
            IEfSubeData subeData,
            IEfSezonData sezonData,
            IDpOgrenciData dpData)
        {
            this.data = data;
            this.hesapData = hesapData;
            this.subeData = subeData;
            this.sezonData = sezonData;
            this.dpData = dpData;
        }

        public EntityOperationResult<Ogrenci> Add(Ogrenci entity)
        {
            var entityOperationResult = new EntityOperationResult<Ogrenci>(entity);

            var validationResults = EntityValidator<Ogrenci>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            // Eğer Öğrenci İletişim için seçildiyse, ceptelefon kontrol ediyoruz.
            if (entity.IletisimKendi && string.IsNullOrEmpty(entity.CepTelefon))
            {
                //IletisimTercihEdlineCepTelefonBosOlamaz
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OgrenciIletisimTercihEdlineCepTelefonBosOlamaz
                });

                return entityOperationResult;
            }

            entity.OlusturulmaTarihi = DateTime.Now;

            var sube = subeData.GetById(Identity.SubeId);

            entity.Hesap = new Hesap
            {
                BagliKurumId = Identity.KurumId,
                ParaBirimId = (int)sube.ParaBirimId,
                HesapBaslik = entity.AdSoyad,
                Ogrenci = entity,
                HesapTurId = 2,
                EtkinMi = true
            };

            entity.EkleyenPersonelId = Identity.PersonelId;
            entity.SubeId = Identity.SubeId;

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

        public EntityOperationResult<Ogrenci> Update(Ogrenci entity)
        {
            var entityOperationResult = new EntityOperationResult<Ogrenci>(entity);

            var validationResults = EntityValidator<Ogrenci>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1 && entity.SubeId != Identity.SubeId)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            // Eğer Öğrenci İletişim için seçildiyse, ceptelefon kontrol ediyoruz.
            if (entity.IletisimKendi && string.IsNullOrEmpty(entity.CepTelefon))
            {
                //IletisimTercihEdlineCepTelefonBosOlamaz
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OgrenciIletisimTercihEdlineCepTelefonBosOlamaz
                });

                return entityOperationResult;
            }

            entity.Hesap = hesapData.GetById(entity.OgrenciId);
            entity.Hesap.HesapBaslik = entity.AdSoyad;

            var returnMessage = data.UpdateWithNested(entity);

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

        public EntityOperationResult<Ogrenci> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<Ogrenci>(null);

            var ogrenci = data.GetById(id);

            if (ogrenci == null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OgrenciBulunamadi
                });

                return entityOperationResult;
            }

            if (Identity.PersonelId != -2 && ogrenci.EkleyenPersonelId != Identity.PersonelId)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.OgrenciyiEkleyenPersonelSilebilir
                });

                return entityOperationResult;
            }

            var newParameters = new List<Parameter>()
            {
                new Parameter("OgrenciId", id),
                 new Parameter("PersonelId", Identity.PersonelId)
            };

            var returnCount = dpData.ExecuteGetRowCount("Ogrenci_Sil", newParameters);

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

        public int GetCount(Expression<Func<Ogrenci, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Ogrenci>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Ogrenci>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public string GetSonOgrenciNo()
        {
            var ogrenciNo = string.Empty;
            var sonOgrenci = GetSonOgrenci();
            var sonSezon = sezonData.GetByWhereCaseByOrderByTakeIncludeMultiple(x => x.KurumId == Identity.KurumId || x.KurumId == null, x => x.SezonAd, 1, true).FirstOrDefault();
            var ilkOgrenciNo = false;

            if (sonOgrenci == null)
            {
                if (sonSezon != null && sonSezon.BitisTarihi.Value.Year >= DateTime.Now.Year)
                {
                    ilkOgrenciNo = true;
                }
            }
            else
            {
                if (sonOgrenci.OgrenciNo.Length > 4)
                {
                    var ogrenciNoInt = Convert.ToInt32(sonOgrenci.OgrenciNo.Substring(4, 5)) + 1;

                    ogrenciNo = $"{sonOgrenci.OgrenciNo.Substring(0, 4)}{ogrenciNoInt.ToString().PadLeft(5, '0')}";
                }
                else
                {
                    ilkOgrenciNo = true;
                }
            }

            if (sonSezon == null)
            {
                sonSezon = new Sezon();

                if (DateTime.Now.Month > 8)
                {
                    sonSezon.BaslangicTarihi = new DateTime(DateTime.Now.Year - 1, 1, 1);
                    sonSezon.BitisTarihi = new DateTime(DateTime.Now.Year, 1, 1);
                }
                else
                {
                    sonSezon.BaslangicTarihi = new DateTime(DateTime.Now.Year, 1, 1);
                    sonSezon.BitisTarihi = new DateTime(DateTime.Now.Year + 1, 1, 1);
                }
            }

            if (ilkOgrenciNo)
            {
                ogrenciNo = $"{sonSezon.BaslangicTarihi.Value.Year.ToString().Substring(2, 2)}{sonSezon.BitisTarihi.Value.Year.ToString().Substring(2, 2)}00001";
            }

            return ogrenciNo;
        }

        public Ogrenci Get(Expression<Func<Ogrenci, bool>> expression = null, params Expression<Func<Ogrenci, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Ogrenci>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Ogrenci>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public Ogrenci GetBilgi(Expression<Func<Ogrenci, bool>> expression = null, params Expression<Func<Ogrenci, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public Ogrenci GetSonOgrenci()
        {
            return data.GetByWhereCaseByOrderByTakeIncludeMultiple(x =>
                x.SubeId == Identity.SubeId,
                x => x.OgrenciNo,
                1,
                true).
            FirstOrDefault();
        }

        public IEnumerable<Ogrenci> List(Expression<Func<Ogrenci, bool>> expression = null, params Expression<Func<Ogrenci, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Ogrenci>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Ogrenci>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
            }
        }

        public EntityPagedDataSource<Ogrenci> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<OgrenciDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("SubeId", Identity.SubeId));

            var entityPagedDataSource = dpData.GetPagedEntities("Ogrenci_Listele", filter, newParameters);

            return entityPagedDataSource;
        }
    }
}
