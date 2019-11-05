using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
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
    public class BankaHesapManager : IBankaHesapService
    {
        IEfBankaHesapData data;
        IEfBankaData bankaData;
        IEfHesapData hesapData;

        public BankaHesapManager(
            IEfBankaHesapData data,
            IEfBankaData bankaData,
            IEfHesapData hesapData)
        {
            this.data = data;
            this.bankaData = bankaData;
            this.hesapData = hesapData;
        }

        public EntityOperationResult<BankaHesap> Add(BankaHesap entity)
        {
            var entityOperationResult = new EntityOperationResult<BankaHesap>(entity);

            var validationResults = EntityValidator<BankaHesap>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.UstHesapId == entity.UstHesapId &&
                x.Aciklama.ToLower() == entity.Aciklama.ToLower() &&
                x.BankaId == entity.BankaId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            var banka = bankaData.GetById(entity.BankaId);

            entity.Hesap = new Hesap
            {
                BagliKurumId = Identity.KurumId,
                UstHesapId = entity.UstHesapId,
                ParaBirimId = entity.ParaBirimId,
                HesapBaslik = $"{entity.Aciklama} ({banka.BankaAd})",
                HesapTurId = 4,
                EtkinMi = true,
                BankaHesap = entity
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

        public EntityOperationResult<BankaHesap> Update(BankaHesap entity)
        {
            var entityOperationResult = new EntityOperationResult<BankaHesap>(entity);

            var validationResults = EntityValidator<BankaHesap>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.UstHesapId == entity.UstHesapId &&
                x.Aciklama.ToLower() == entity.Aciklama.ToLower() &&
                x.BankaHesapId != entity.BankaHesapId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            var banka = bankaData.GetById(entity.BankaId);

            entity.Hesap = hesapData.GetByWhereCaseIncludeMultipleFirstOrDefault(x =>
                x.HesapId == entity.BankaHesapId,
                y => y.UstHesap.Sube);

            entity.Hesap.HesapBaslik = $"{entity.Aciklama} ({banka.BankaAd})";

            string returnMessage = string.Empty;

            if (Identity.KurumId == -1)
            {
                returnMessage = data.UpdateWithNested(entity);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    if (!yetkiliSubeIdler.Contains(entity.UstHesapId))
                    {
                        entityOperationResult.MessageInfos.Add(new MessageInfo
                        {
                            MessageInfoType = MessageInfoType.Error,
                            Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                        });

                        return entityOperationResult;
                    }
                }
                else
                {
                    if (entity.UstHesap.Sube.KurumId != Identity.KurumId)
                    {
                        entityOperationResult.MessageInfos.Add(new MessageInfo
                        {
                            MessageInfoType = MessageInfoType.Error,
                            Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                        });

                        return entityOperationResult;
                    }
                }
            }

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

        public EntityOperationResult<BankaHesap> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<BankaHesap, bool>> expression = null)
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

                    var predicate = PredicateBuilder.True<BankaHesap>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.UstHesapId)).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var predicate = PredicateBuilder.True<BankaHesap>();

                    predicate = predicate.And(x => x.UstHesap.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public BankaHesap Get(Expression<Func<BankaHesap, bool>> expression = null, params Expression<Func<BankaHesap, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<BankaHesap>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.UstHesapId)).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<BankaHesap>();

                    predicate = predicate.And(x => x.UstHesap.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<BankaHesap> List(Expression<Func<BankaHesap, bool>> expression = null, params Expression<Func<BankaHesap, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<BankaHesap>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.UstHesapId)).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<BankaHesap>();

                    predicate = predicate.And(x => x.UstHesap.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
            }
        }

        public EntityPagedDataSource<BankaHesap> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<BankaHesap> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<BankaHesap>
                {
                    data = data.GetAllIncludeMultiple(
                        y=> y.Banka, 
                        y=> y.ParaBirim, 
                        y=> y.UstHesap)
                };
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {

                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    entityPagedDataSource = new EntityPagedDataSource<BankaHesap>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x => 
                            yetkiliSubeIdler.Contains(x.UstHesapId), 
                            y => y.Banka,
                            y => y.ParaBirim,
                            y => y.UstHesap)
                    };
                }
                else
                {
                    entityPagedDataSource = new EntityPagedDataSource<BankaHesap>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x => 
                            x.UstHesap.Sube.KurumId == Identity.KurumId,
                            y => y.Banka,
                            y => y.ParaBirim,
                            y => y.UstHesap)
                    };
                }
            }

            return entityPagedDataSource;
        }
    }
}
