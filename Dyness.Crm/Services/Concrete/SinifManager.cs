using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Data.Abstract.EntityFramework;
using Entities.Concrete;
using Services.Abstract;
using Services.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Core.Services.Helpers;
using Data.Abstract.Dapper;

namespace Services.Concrete
{
    public class SinifManager : ISinifService
    {
        IEfSinifData data;
        IDpSinifData dpData;

        public SinifManager(IEfSinifData data, IDpSinifData dpData)
        {
            this.data = data;
            this.dpData = dpData;
        }

        public EntityOperationResult<Sinif> Add(Sinif entity)
        {
            var entityOperationResult = new EntityOperationResult<Sinif>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Sinif>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => x.SinifAd.ToLower() == entity.SinifAd.ToLower() && x.SubeId == entity.SubeId && x.SezonId == entity.SezonId);

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

        public EntityOperationResult<Sinif> Update(Sinif entity)
        {
            var entityOperationResult = new EntityOperationResult<Sinif>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Sinif>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.SinifAd.ToLower() == entity.SinifAd.ToLower() &&
                x.SubeId == entity.SubeId &&
                x.SinifId != entity.SinifId);

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

        public EntityOperationResult<Sinif> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<Sinif>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

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
                Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public Sinif Get(Expression<Func<Sinif, bool>> expression = null, params Expression<Func<Sinif, object>>[] includes)
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
                    var predicate = PredicateBuilder.True<Sinif>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Sinif>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public int GetCount(Expression<Func<Sinif, bool>> expression = null)
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
                    var predicate = PredicateBuilder.True<Sinif>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Sinif>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public IEnumerable<Sinif> List(Expression<Func<Sinif, bool>> expression = null, params Expression<Func<Sinif, object>>[] includes)
        {
            var newIncludes = new Expression<Func<Sinif, object>>[includes?.Length + 1 ?? 0];

            if (includes != null)
            {
                for (int i = 0; i < includes.Length; i++)
                {
                    newIncludes[i] = includes[i];
                }
            }

            newIncludes[newIncludes.Length - 1] = y => y.Sube.Kurum;

            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, newIncludes);
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (!personelSubeYetkiler.Any())
                {
                    var predicate = PredicateBuilder.True<Sinif>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, newIncludes);
                }
                else
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    var predicate = PredicateBuilder.True<Sinif>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, newIncludes);
                }
            }
        }

        public IEnumerable<Sinif> SinifListele(IEnumerable<Parameter> parameters)
        {
            return dpData.GetEntities("Sinif_Listele", parameters);
        }

        public EntityPagedDataSource<Sinif> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<Sinif> entityPagedDataSource = null;

            var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

            if (!personelSubeYetkiler.Any())
            {
                entityPagedDataSource = new EntityPagedDataSource<Sinif>
                {
                    data = data.GetByWhereCaseIncludeMultiple(
                        x => x.Sube.KurumId == Identity.KurumId,
                        y => y.Sube,
                        y => y.Sezon,
                        y => y.Brans,
                        y => y.SinifTur,
                        y => y.SinifSeviye,
                        y => y.SinifSeans,
                        y => y.Derslik)
                };
            }
            else
            {
                var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId);

                entityPagedDataSource = new EntityPagedDataSource<Sinif>
                {
                    data = data.GetByWhereCaseIncludeMultiple(
                   x => yetkiliSubeIdler.Contains(x.SubeId),
                   y => y.Sube,
                   y => y.Sezon,
                   y => y.Brans,
                   y => y.SinifTur,
                   y => y.SinifSeviye,
                   y => y.SinifSeans,
                   y => y.Derslik)
                };
            }

            return entityPagedDataSource;
        }
    }
}
