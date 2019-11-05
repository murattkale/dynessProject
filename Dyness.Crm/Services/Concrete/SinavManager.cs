using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Core.Properties;
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
    public class SinavManager : ISinavService
    {
        IEfSinavData data;
        IDpSinavData dpData;

        public SinavManager(IEfSinavData data, IDpSinavData dpData)
        {
            this.data = data;
            this.dpData = dpData;
        }

        public EntityOperationResult<Sinav> Add(Sinav entity)
        {
            var entityOperationResult = new EntityOperationResult<Sinav>(entity);

            var validationResults = EntityValidator<Sinav>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var addList = new List<SinavKitapcik>();

            foreach (var sinavKitapcik in entity.SinavKitapciklar)
            {
                if (!string.IsNullOrEmpty(sinavKitapcik.Baslik))
                    addList.Add(sinavKitapcik);
            }

            entity.SinavKitapciklar = addList;

            var returnMessage = data.AddWithNestedLists(entity);

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
                Message = ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Sinav> Update(Sinav entity)
        {
            var entityOperationResult = new EntityOperationResult<Sinav>(entity);

            var validationResults = EntityValidator<Sinav>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

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
                Message = ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<Sinav> DeleteById(int id)
        {
            var newParameters = new List<Parameter>()
            {
                new Parameter("SinavId", id)
            };

            var entityOperationResult = new EntityOperationResult<Sinav>(null);

            var returnCount = dpData.ExecuteGetRowCount("Sinav_Sil", newParameters);

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
                Message = ServiceNoticesResources.BasariylaSilindi
            });

            return entityOperationResult;
        }

        public int GetCount(Expression<Func<Sinav, bool>> expression = null)
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

                    var predicate = PredicateBuilder.True<Sinav>();

                    predicate = predicate.And(x => 
                        yetkiliSubeIdler.Count(y => x.SinavSubeler.Select(z => z.SubeId).Contains(y)) > 0).
                    And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Sinav>();

                    predicate = predicate.And(x => x.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public Sinav Get(Expression<Func<Sinav, bool>> expression = null, params Expression<Func<Sinav, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<Sinav>();

                    predicate = predicate.And(x => 
                        yetkiliSubeIdler.Count(y => x.SinavSubeler.Select(z => z.SubeId).Contains(y)) > 0).
                    And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Sinav>();

                    predicate = predicate.And(x => x.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<Sinav> List(Expression<Func<Sinav, bool>> expression = null, params Expression<Func<Sinav, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<Sinav>();

                    predicate = predicate.And(x => 
                            yetkiliSubeIdler.Count(y => x.SinavSubeler.
                        Select(z => z.SubeId).Contains(y)) > 0).
                        And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<Sinav>();

                    predicate = predicate.And(x => x.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
            }
        }

        public IEnumerable<Sinav> SinavListele(IEnumerable<Parameter> parameters)
        {
            return dpData.GetEntities("Sinav_Listele", parameters);
        }

        public EntityPagedDataSource<Sinav> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<Sinav> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<Sinav>
                {
                    data = data.GetAllIncludeMultiple(x => x.Kurum, y => y.SinavTur)
                };
            }
            else
            {
                var personelSubeYetkiler = PersonelSubeYetkiService.Get(Identity.PersonelId);

                if (personelSubeYetkiler.Any())
                {
                    var yetkiliSubeIdler = personelSubeYetkiler.Select(x => x.SubeId).ToList();

                    entityPagedDataSource = new EntityPagedDataSource<Sinav>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x => 
                            yetkiliSubeIdler.Count(y => x.SinavSubeler.Select(z => z.SubeId).Contains(y)) > 0, 
                            y => y.Kurum, 
                            y => y.SinavTur)
                    };
                }
                else
                {
                    entityPagedDataSource = new EntityPagedDataSource<Sinav>
                    {
                        data = data.GetByWhereCaseIncludeMultiple(x => 
                            x.KurumId == Identity.KurumId, 
                            y => y.Kurum, 
                            y => y.SinavTur)
                    };
                }
            }

            return entityPagedDataSource;
        }
    }
}
