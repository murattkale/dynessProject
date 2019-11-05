using Services.Abstract;
using System;
using System.Collections.Generic;
using Entities.Concrete;
using System.Linq.Expressions;
using Data.Abstract.EntityFramework;
using Core.Entities.Dto;
using System.Linq;
using Services.Validation;
using Core.CrossCuttingConcerns.Security;
using Core.General;
using Core.Services.Helpers;

namespace Services.Concrete
{
    public class SubeManager : ISubeService
    {
        IEfSubeData data;
        IEfSinifData sinifData;
        IEfPersonelData personelData;
        IEfHesapData hesapData;
        IEfParaBirimData paraBirimData;

        public SubeManager(
            IEfSubeData data,
            IEfSinifData sinifData,
            IEfPersonelData personelData,
            IEfHesapData hesapData,
            IEfParaBirimData paraBirimData)
        {
            this.data = data;
            this.sinifData = sinifData;
            this.personelData = personelData;
            this.hesapData = hesapData;
            this.paraBirimData = paraBirimData;
        }

        public EntityOperationResult<Sube> Add(Sube entity)
        {
            EntityOperationResult<Sube> entityOperationResult = new EntityOperationResult<Sube>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Sube>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.SubeAd.ToLower() == entity.SubeAd.ToLower() &&
            x.SubeId == entity.SubeId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            var paraBirim = paraBirimData.GetById((int)entity.ParaBirimId);

            entity.Hesap = new Hesap
            {
                BagliKurumId = entity.KurumId,
                ParaBirimId = (int)entity.ParaBirimId,
                Sube = entity,
                HesapTurId = 1,
                EtkinMi = true,
                HesapBaslik = entity.SubeAd,
                AltHesaplar = new List<Hesap>
                {
                    new Hesap
                    {
                        UstHesap = entity.Hesap,
                        BagliKurumId = entity.KurumId,
                        ParaBirimId = (int)entity.ParaBirimId,
                        HesapTurId = 4,
                        EtkinMi = true,
                        HesapBaslik = $"{paraBirim.Kod} - Kasa"
                    }
                }
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
                Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Sube> Update(Sube entity)
        {
            EntityOperationResult<Sube> entityOperationResult = new EntityOperationResult<Sube>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Sube>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
            x.SubeAd.ToLower() == entity.SubeAd.ToLower() &&
            x.KurumId == entity.KurumId &&
            x.SubeId != entity.SubeId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            entity.Hesap = hesapData.GetById(entity.SubeId);
            entity.Hesap.HesapBaslik = entity.SubeAd;

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
                Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Sube> DeleteById(int id)
        {
            EntityOperationResult<Sube> entityOperationResult = new EntityOperationResult<Sube>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = sinifData.GetByWhereCaseCount(x => x.SubeId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SubeyiSilebilmekIcinSiniflar
                });

                return entityOperationResult;
            }

            checkExistsCount = personelData.GetByWhereCaseCount(x => x.SubeId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.SubeyiSilebilmekIcinSiniflar
                });

                return entityOperationResult;
            }

            var entity = data.GetById(id);

            var returnMessage = data.DeleteWithNested(entity);

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

        public int GetCount(Expression<Func<Sube, bool>> expression = null)
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
                    var predicate = PredicateBuilder.True<Sube>();

                    predicate = predicate.And(x => x.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Sube>();

                    predicate = predicate.And(x =>
                        x.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Contains(x.SubeId)).
                    And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public string GetSonSubeKod()
        {
            var subeKod = string.Empty;
            var sonSube = data.GetByWhereCaseByOrderByTakeIncludeMultiple(x => x.KurumId == Identity.KurumId, x => x.SubeId, 1, true).FirstOrDefault();

            if (sonSube == null)
            {
                subeKod = "0001";
            }
            else
            {
                var subeKodInt = Convert.ToInt32(sonSube.Kod) + 1;
                subeKod = $"{subeKodInt.ToString().PadLeft(4, '0')}";
            }

            return subeKod;
        }

        public Sube Get(Expression<Func<Sube, bool>> expression = null, params Expression<Func<Sube, object>>[] includes)
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
                    var predicate = PredicateBuilder.True<Sube>();

                    predicate = predicate.And(x => x.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Sube>();

                    predicate = predicate.And(x =>
                        x.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Contains(x.SubeId)).
                    And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<Sube> List(Expression<Func<Sube, bool>> expression = null, params Expression<Func<Sube, object>>[] includes)
        {
            var newIncludes = new Expression<Func<Sube, object>>[includes == null ? 0 : includes.Length + 1];

            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    newIncludes[i] = includes[i];
                }
            }

            newIncludes[newIncludes.Length - 1] = y => y.Kurum;

            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, newIncludes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    var predicate = PredicateBuilder.True<Sube>();

                    predicate = predicate.And(x => x.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, newIncludes);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Sube>();

                    predicate = predicate.And(x =>
                        x.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Contains(x.SubeId)).
                    And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, newIncludes);
                }
            }
        }

        public EntityPagedDataSource<Sube> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<Sube> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<Sube>
                {
                    data = data.GetAllIncludeMultiple(
                        y => y.Kurum,
                        y => y.Sehir,
                        y => y.ParaBirim)
                };
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    entityPagedDataSource = new EntityPagedDataSource<Sube>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x =>
                            x.KurumId == Identity.KurumId,
                            y => y.Kurum,
                            y => y.Sehir,
                            y => y.ParaBirim)
                    };
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    entityPagedDataSource = new EntityPagedDataSource<Sube>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x =>
                            x.KurumId == Identity.KurumId &&
                            yetkiliSubeIdler.Contains(x.SubeId),
                            y => y.Kurum,
                            y => y.Sehir,
                            y => y.ParaBirim)
                    };
                }
            }

            return entityPagedDataSource;
        }
    }
}
