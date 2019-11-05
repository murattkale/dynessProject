using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
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
    public class PersonelYetkiGrupManager : IPersonelYetkiGrupService
    {
        IEfPersonelYetkiGrupData data;
        IEfPersonelData personelData;

        public PersonelYetkiGrupManager(IEfPersonelYetkiGrupData data, IEfPersonelData personelData)
        {
            this.data = data;
            this.personelData = personelData;
        }

        public EntityOperationResult<PersonelYetkiGrup> Add(PersonelYetkiGrup entity)
        {
            EntityOperationResult<PersonelYetkiGrup> entityOperationResult = new EntityOperationResult<PersonelYetkiGrup>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<PersonelYetkiGrup>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1)
            {
                entity.KurumId = Identity.KurumId;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => 
                x.KurumId == entity.KurumId &&
                x.PersonelYetkiGrupAd.ToLower() == entity.PersonelYetkiGrupAd.ToLower());

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

        public EntityOperationResult<PersonelYetkiGrup> Update(PersonelYetkiGrup entity)
        {
            EntityOperationResult<PersonelYetkiGrup> entityOperationResult = new EntityOperationResult<PersonelYetkiGrup>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<PersonelYetkiGrup>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1 &&
                entity.KurumId != Identity.KurumId ||
                entity.KurumId == -1)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => 
                x.KurumId == entity.KurumId &&
                x.PersonelYetkiGrupAd.ToLower() == entity.PersonelYetkiGrupAd.ToLower() && 
                x.PersonelYetkiGrupId != entity.PersonelYetkiGrupId);

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

        public EntityOperationResult<PersonelYetkiGrup> DeleteById(int id)
        {
            EntityOperationResult<PersonelYetkiGrup> entityOperationResult = new EntityOperationResult<PersonelYetkiGrup>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var foreignKeyCheckCount = personelData.GetByWhereCaseCount(x => x.PersonelYetkiGrupId == id);

            if (foreignKeyCheckCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.PersonelYetkiGrubuSilebilmekIcin
                });

                return entityOperationResult;
            }

            var entity = data.GetById(id);

            if (Identity.KurumId != -1 &&
                entity.KurumId != Identity.KurumId ||
                entity.KurumId == -1)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

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

        public int GetCount(Expression<Func<PersonelYetkiGrup, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<PersonelYetkiGrup>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public PersonelYetkiGrup Get(Expression<Func<PersonelYetkiGrup, bool>> expression = null, params Expression<Func<PersonelYetkiGrup, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<PersonelYetkiGrup>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<PersonelYetkiGrup> List(Expression<Func<PersonelYetkiGrup, bool>> expression = null, params Expression<Func<PersonelYetkiGrup, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<PersonelYetkiGrup>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<PersonelYetkiGrup> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<PersonelYetkiGrup> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<PersonelYetkiGrup>
                {
                    data = data.GetAll()
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<PersonelYetkiGrup>
                {
                    data = data.GetByWhereCase(x =>
                        x.KurumId == null ||
                        x.KurumId == Identity.KurumId)
                };
            }

            return entityPagedDataSource;
        }
    }
}
