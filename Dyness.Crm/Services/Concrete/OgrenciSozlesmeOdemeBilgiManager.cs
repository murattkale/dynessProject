using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.General;
using Core.Services;
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
    public class OgrenciSozlesmeOdemeBilgiManager : IOgrenciSozlesmeOdemeBilgiService
    {
        IEfOgrenciSozlesmeOdemeBilgiData data;

        public OgrenciSozlesmeOdemeBilgiManager(IEfOgrenciSozlesmeOdemeBilgiData data)
        {
            this.data = data;
        }

        public EntityOperationResult<OgrenciSozlesmeOdemeBilgi> Add(OgrenciSozlesmeOdemeBilgi entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<OgrenciSozlesmeOdemeBilgi> Update(OgrenciSozlesmeOdemeBilgi entity)
        {
            var entityOperationResult = new EntityOperationResult<OgrenciSozlesmeOdemeBilgi>(entity);

            var validationResults = EntityValidator<OgrenciSozlesmeOdemeBilgi>.ValidateEntity(entity).ToList();

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
                Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        EntityOperationResult<OgrenciSozlesmeOdemeBilgi> IServiceModel<OgrenciSozlesmeOdemeBilgi>.DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<OgrenciSozlesmeOdemeBilgi, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public OgrenciSozlesmeOdemeBilgi Get(Expression<Func<OgrenciSozlesmeOdemeBilgi, bool>> expression = null, params Expression<Func<OgrenciSozlesmeOdemeBilgi, object>>[] includes)
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

                    var predicate = PredicateBuilder.True<OgrenciSozlesmeOdemeBilgi>();

                    predicate = predicate.And(x => yetkiliSubeIdler.Contains(x.OgrenciSozlesme.SubeId)).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
                else
                {
                    var predicate = PredicateBuilder.True<OgrenciSozlesmeOdemeBilgi>();

                    predicate = predicate.And(x => x.OgrenciSozlesme.Sube.KurumId == Identity.KurumId).And(expression);

                    return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
                }
            }
        }

        public IEnumerable<OgrenciSozlesmeOdemeBilgi> List(Expression<Func<OgrenciSozlesmeOdemeBilgi, bool>> expression = null, params Expression<Func<OgrenciSozlesmeOdemeBilgi, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        EntityPagedDataSource<OgrenciSozlesmeOdemeBilgi> IServiceModel<OgrenciSozlesmeOdemeBilgi>.EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
