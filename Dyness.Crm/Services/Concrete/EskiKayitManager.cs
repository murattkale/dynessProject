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
    public class EskiKayitManager : IEskiKayitService
    {
        IEfEskiKayitData data;

        public EskiKayitManager(IEfEskiKayitData data)
        {
            this.data = data;
        }

        public EntityOperationResult<EskiKayit> Add(EskiKayit entity)
        {
            var entityOperationResult = new EntityOperationResult<EskiKayit>(entity);

            var validationResults = EntityValidator<EskiKayit>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => 
                x.OgrenciTckn == entity.OgrenciTckn && 
                x.OgrenciAdi == entity.OgrenciAdi && 
                x.OgrenciSoyadi == entity.OgrenciSoyadi && 
                x.SinifSeviyesi == entity.SinifSeviyesi &&
                x.Sezon == entity.Sezon &&
                x.Brans == entity.Brans &&
                x.Sinif == entity.Sinif &&
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

        public EntityOperationResult<EskiKayit> Update(EskiKayit entity)
        {
            var entityOperationResult = new EntityOperationResult<EskiKayit>(entity);

            var validationResults = EntityValidator<EskiKayit>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.OgrenciTckn == entity.OgrenciTckn &&
                x.OgrenciAdi == entity.OgrenciAdi &&
                x.OgrenciSoyadi == entity.OgrenciSoyadi &&
                x.SinifSeviyesi == entity.SinifSeviyesi &&
                x.Sezon == entity.Sezon &&
                x.Brans == entity.Brans &&
                x.Sinif == entity.Sinif &&
                x.SubeId == entity.SubeId && 
                x.EskiKayitId != entity.EskiKayitId);

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

        public EntityOperationResult<EskiKayit> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<EskiKayit>
            {
                MessageInfos = new List<MessageInfo>()
            };

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

        public int GetCount(Expression<Func<EskiKayit, bool>> expression = null)
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
                    var predicate = PredicateBuilder.True<EskiKayit>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<EskiKayit>();

                    predicate = predicate.And(x =>
                        x.Sube.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Count(y => y == x.SubeId) > 0).
                    And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public EskiKayit Get(Expression<Func<EskiKayit, bool>> expression = null, params Expression<Func<EskiKayit, object>>[] includes)
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
                    var predicate = PredicateBuilder.True<EskiKayit>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<EskiKayit>();

                    predicate = predicate.And(x =>
                        x.Sube.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Count(y => y == x.SubeId) > 0).
                    And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<EskiKayit> List(Expression<Func<EskiKayit, bool>> expression = null, params Expression<Func<EskiKayit, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    var predicate = PredicateBuilder.True<EskiKayit>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<EskiKayit>();

                    predicate = predicate.And(x =>
                        x.Sube.KurumId == Identity.KurumId &&
                        yetkiliSubeIdler.Count(y => y == x.SubeId) > 0).
                    And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
            }
        }

        public EntityPagedDataSource<EskiKayit> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<EskiKayit> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<EskiKayit>
                {
                    data = data.GetAllIncludeMultiple(y => y.Sube)
                };
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    entityPagedDataSource = new EntityPagedDataSource<EskiKayit>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x => x.Sube.KurumId == Identity.KurumId, y => y.Sube)
                    };
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    entityPagedDataSource = new EntityPagedDataSource<EskiKayit>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x => yetkiliSubeIdler.Count(y => y == x.SubeId) > 0, y => y.Sube)
                    };
                }
            }

            return entityPagedDataSource;
        }
    }
}
