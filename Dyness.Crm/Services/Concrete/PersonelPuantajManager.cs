using Core.Entities.Dto;
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
    public class PersonelPuantajManager : IPersonelPuantajService
    {
        IEfPersonelPuantajData data;

        public PersonelPuantajManager(IEfPersonelPuantajData data)
        {
            this.data = data;
        }

        public EntityOperationResult<PersonelPuantaj> Add(PersonelPuantaj entity)
        {
            EntityOperationResult<PersonelPuantaj> entityOperationResult = new EntityOperationResult<PersonelPuantaj>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<PersonelPuantaj>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var entityExists = data.GetByWhereCaseIncludeMultiple(
                x =>
                x.PuantajAy == entity.PuantajAy &&
                x.PuantajYil == entity.PuantajYil &&
                x.PersonelId == entity.PersonelId,
                y => y.Personel,
                y => y.PersonelPuantajGunlukler.Select(z => z.PersonelPuantajGunlukDurum)).
                FirstOrDefault();

            if (entityExists != null)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Success,
                    Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
                });

                entityOperationResult.Status = true;
                entityOperationResult.Model = entityExists;

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

            entityOperationResult.Model = data.GetByWhereCaseIncludeMultiple(
                x =>
                x.PersonelPuantajId == entity.PersonelPuantajId,
                y => y.Personel,
                y => y.PersonelPuantajGunlukler.Select(z => z.PersonelPuantajGunlukDurum)).
                FirstOrDefault();

            entityOperationResult.Model.PersonelPuantajGunlukler = entityOperationResult.Model.PersonelPuantajGunlukler.OrderBy(x => x.Gun).ToList(); 

            entityOperationResult.Status = true;
            entityOperationResult.MessageInfos.Add(new MessageInfo
            {
                MessageInfoType = MessageInfoType.Success,
                Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<PersonelPuantaj> Update(PersonelPuantaj entity)
        {
            EntityOperationResult<PersonelPuantaj> entityOperationResult = new EntityOperationResult<PersonelPuantaj>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<PersonelPuantaj>.ValidateEntity(entity).ToList();

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
                Message = Core.Properties.ServiceNoticesResources.BasariylaEklendi
            });

            return entityOperationResult;
        }

        public EntityOperationResult<PersonelPuantaj> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<PersonelPuantaj, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public PersonelPuantaj Get(Expression<Func<PersonelPuantaj, bool>> expression = null, params Expression<Func<PersonelPuantaj, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<PersonelPuantaj> List(Expression<Func<PersonelPuantaj, bool>> expression = null, params Expression<Func<PersonelPuantaj, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<PersonelPuantaj> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<PersonelPuantaj>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
