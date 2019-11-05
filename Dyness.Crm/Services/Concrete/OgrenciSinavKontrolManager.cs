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
    public class OgrenciSinavKontrolManager : IOgrenciSinavKontrolService
    {
        IEfOgrenciSinavKontrolData data;
        IDpOgrenciSinavKontrolData dpData;

        public OgrenciSinavKontrolManager(IEfOgrenciSinavKontrolData data, IDpOgrenciSinavKontrolData dpData)
        {
            this.data = data;
            this.dpData = dpData;
        }

        public EntityOperationResult<OgrenciSinavKontrol> Add(OgrenciSinavKontrol entity)
        {
            var entityOperationResult = new EntityOperationResult<OgrenciSinavKontrol>(entity);

            var validationResults = EntityValidator<OgrenciSinavKontrol>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.SinavKitapcikId == entity.SinavKitapcikId &&
                x.AdSoyad == entity.AdSoyad &&
                x.TcKimlikNo == entity.TcKimlikNo);

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

        public EntityOperationResult<OgrenciSinavKontrol> Update(OgrenciSinavKontrol entity)
        {
            var entityOperationResult = new EntityOperationResult<OgrenciSinavKontrol>(entity);

            var validationResults = EntityValidator<OgrenciSinavKontrol>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

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

        public EntityOperationResult<OgrenciSinavKontrol> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<OgrenciSinavKontrol, bool>> expression = null)
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

                    var predicate = PredicateBuilder.True<OgrenciSinavKontrol>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
                else
                {
                    var predicate = PredicateBuilder.True<OgrenciSinavKontrol>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseCount(predicate);
                }
            }
        }

        public OgrenciSinavKontrol Get(Expression<Func<OgrenciSinavKontrol, bool>> expression = null, params Expression<Func<OgrenciSinavKontrol, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<OgrenciSinavKontrol>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<OgrenciSinavKontrol>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<OgrenciSinavKontrol> List(Expression<Func<OgrenciSinavKontrol, bool>> expression = null, params Expression<Func<OgrenciSinavKontrol, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<OgrenciSinavKontrol>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<OgrenciSinavKontrol>();

                    predicate = predicate.And(x => x.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultiple(predicate, includes);
                }
            }
        }

        public EntityPagedDataSource<OgrenciSinavKontrol> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<OgrenciSinavKontrolDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = dpData.GetPagedEntities("OgrenciSinavKontrol_Listele", filter, parameters);

            return entityPagedDataSource;
        }
    }
}
