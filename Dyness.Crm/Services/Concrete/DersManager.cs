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
    public class DersManager : IDersService
    {
        IEfDersData data;
        IEfOgrenciSozlesmeDersSecimData ogrenciSozlesmeDersSecimData;
        IEfPersonelData personelData;
        IEfYayinData yayinData;

        public DersManager(
            IEfDersData data,
            IEfOgrenciSozlesmeDersSecimData ogrenciSozlesmeDersSecimData,
            IEfPersonelData personelData,
            IEfYayinData yayinData)
        {
            this.data = data;
            this.ogrenciSozlesmeDersSecimData = ogrenciSozlesmeDersSecimData;
            this.personelData = personelData;
            this.yayinData = yayinData;
        }

        public EntityOperationResult<Ders> Add(Ders entity)
        {
            var entityOperationResult = new EntityOperationResult<Ders>(entity);

            var validationResults = EntityValidator<Ders>.ValidateEntity(entity).ToList();

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
                x.DersAd.ToLower() == entity.DersAd.ToLower());

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            var returnMessage = data.AddWithNestedLists(entity, entity.BransDersler);

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

        public EntityOperationResult<Ders> Update(Ders entity)
        {
            var entityOperationResult = new EntityOperationResult<Ders>(entity);

            var validationResults = EntityValidator<Ders>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            if (Identity.KurumId != -1 && entity.KurumId != Identity.KurumId)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.YetkisizIslem
                });

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.KurumId == entity.KurumId &&
                x.DersAd.ToLower() == entity.DersAd.ToLower() &&
                x.DersId != entity.DersId);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            var returnMessage = data.UpdateWithNestedLists(entity, entity.BransDersler);

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

        public EntityOperationResult<Ders> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<Ders>();

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = yayinData.GetByWhereCaseCount(x => x.DersId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.DersiSilebilmekIcinYayin
                });

                return entityOperationResult;
            }

            checkExistsCount = personelData.GetByWhereCaseCount(x => x.DersId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.DersiSilebilmekIcinPersonel
                });

                return entityOperationResult;
            }

            checkExistsCount = ogrenciSozlesmeDersSecimData.GetByWhereCaseCount(x => x.DersId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.DersiSilebilmekIcinOgrenciOzelDersSecim
                });

                return entityOperationResult;
            }

            var entity = data.GetById(id);

            if (Identity.KurumId != -1 && entity.KurumId != Identity.KurumId)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = ServiceNoticesResources.YetkisizIslem
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
                Message = ServiceNoticesResources.BasariylaSilindi
            });

            return entityOperationResult;
        }

        public int GetCount(Expression<Func<Ders, bool>> expression = null)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseCount(expression);
            }
            else
            {
                var predicate = PredicateBuilder.True<Ders>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseCount(predicate);
            }
        }

        public Ders Get(Expression<Func<Ders, bool>> expression = null, params Expression<Func<Ders, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<Ders>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression).And(expression);

                return data.GetByWhereCaseIncludeMultipleFirstOrDefault(predicate, includes);
            }
        }

        public IEnumerable<Ders> List(Expression<Func<Ders, bool>> expression = null, params Expression<Func<Ders, object>>[] includes)
        {
            if (Identity.KurumId == -1)
            {
                return data.GetByWhereCaseIncludeMultiple(expression, includes);
            }
            else
            {
                var predicate = PredicateBuilder.True<Ders>();

                predicate = predicate.And(x => x.KurumId == null || x.KurumId == Identity.KurumId).And(expression);

                return data.GetByWhereCaseIncludeMultiple(predicate, includes);
            }
        }

        public EntityPagedDataSource<Ders> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            EntityPagedDataSource<Ders> entityPagedDataSource = null;

            if (Identity.KurumId == -1)
            {
                entityPagedDataSource = new EntityPagedDataSource<Ders>
                {
                    data = data.GetAll()
                };
            }
            else
            {
                entityPagedDataSource = new EntityPagedDataSource<Ders>
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
