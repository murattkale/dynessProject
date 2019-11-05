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
    public class PersonelPuantajGunlukDurumManager : IPersonelPuantajGunlukDurumService
    {
        IEfPersonelPuantajGunlukDurumData data;

        public PersonelPuantajGunlukDurumManager(IEfPersonelPuantajGunlukDurumData data)
        {
            this.data = data;
        }

        public EntityOperationResult<PersonelPuantajGunlukDurum> Add(PersonelPuantajGunlukDurum entity)
        {
            var entityOperationResult = new EntityOperationResult<PersonelPuantajGunlukDurum>(entity);

            var validationResults = EntityValidator<PersonelPuantajGunlukDurum>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => x.PersonelPuantajGunlukDurumAd.ToLower() == entity.PersonelPuantajGunlukDurumAd.ToLower());

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

        public EntityOperationResult<PersonelPuantajGunlukDurum> Update(PersonelPuantajGunlukDurum entity)
        {
            var entityOperationResult = new EntityOperationResult<PersonelPuantajGunlukDurum>(entity);

            var validationResults = EntityValidator<PersonelPuantajGunlukDurum>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => x.PersonelPuantajGunlukDurumAd.ToLower() == entity.PersonelPuantajGunlukDurumAd.ToLower() && x.PersonelPuantajGunlukDurumId != entity.PersonelPuantajGunlukDurumId);

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

        public EntityOperationResult<PersonelPuantajGunlukDurum> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<PersonelPuantajGunlukDurum, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public PersonelPuantajGunlukDurum Get(
            Expression<Func<PersonelPuantajGunlukDurum, bool>> expression = null,
            params Expression<Func<PersonelPuantajGunlukDurum, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<PersonelPuantajGunlukDurum> List(
            Expression<Func<PersonelPuantajGunlukDurum, bool>> expression = null,
            params Expression<Func<PersonelPuantajGunlukDurum, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<PersonelPuantajGunlukDurum> EntityPagedDataSource(
            EntityPagedDataSourceFilter filter,
            IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<PersonelPuantajGunlukDurum>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
