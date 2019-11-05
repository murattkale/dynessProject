using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
using Core.Properties;
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
    public class SinavKitapcikManager : ISinavKitapcikService
    {
        IEfSinavKitapcikData data;

        public SinavKitapcikManager(IEfSinavKitapcikData data)
        {
            this.data = data;
        }

        public EntityOperationResult<SinavKitapcik> Add(SinavKitapcik entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SinavKitapcik> Update(SinavKitapcik entity)
        {
            var entityOperationResult = new EntityOperationResult<SinavKitapcik>(entity);

            var validationResults = EntityValidator<SinavKitapcik>.ValidateEntity(entity).ToList();

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
                Message = ServiceNoticesResources.BasariylaGuncellendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<SinavKitapcik> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<SinavKitapcik, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinavKitapcik>();

                predicate = predicate.And(x => x.Sinav.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public SinavKitapcik Get(Expression<Func<SinavKitapcik, bool>> expression = null, params Expression<Func<SinavKitapcik, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinavKitapcik>();

                predicate = predicate.And(x => x.Sinav.KurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<SinavKitapcik> List(Expression<Func<SinavKitapcik, bool>> expression = null, params Expression<Func<SinavKitapcik, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<SinavKitapcik>();

                predicate = predicate.And(x => x.Sinav.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<SinavKitapcik> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<SinavKitapcik> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<SinavKitapcik>
                {
                    data = data.GetAll()
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<SinavKitapcik>
                {
                    data = data.GetByWhereCase(x => x.Sinav.KurumId == Identity.KurumId)
                };
            }

            return entityPagedDataSource;
        }
    }
}
