using Core.CrossCuttingConcerns.Security;
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
    public class ServisManager : IServisService
    {
        IEfServisData data;
        IEfOgrenciSozlesmeData ogrenciSozlesmeData;

        public ServisManager(IEfServisData data, IEfOgrenciSozlesmeData ogrenciSozlesmeData)
        {
            this.data = data;
            this.ogrenciSozlesmeData = ogrenciSozlesmeData;
        }

        public EntityOperationResult<Servis> Add(Servis entity)
        {
            EntityOperationResult<Servis> entityOperationResult = new EntityOperationResult<Servis>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Servis>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }
            var checkExistsCount = data.GetByWhereCaseCount(x => x.SubeId == Identity.SubeId && x.ServisPlaka.ToLower() == entity.ServisPlaka.ToLower());

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

        public EntityOperationResult<Servis> Update(Servis entity)
        {
            EntityOperationResult<Servis> entityOperationResult = new EntityOperationResult<Servis>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

            var validationResults = EntityValidator<Servis>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(
                x => 
                x.SubeId == Identity.SubeId &&
                x.ServisPlaka.ToLower() == entity.ServisPlaka.ToLower() &&
                x.ServisId != entity.ServisId);

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

        public EntityOperationResult<Servis> DeleteById(int id)
        {
            EntityOperationResult<Servis> entityOperationResult = new EntityOperationResult<Servis>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = ogrenciSozlesmeData.GetByWhereCaseCount(x => x.ServisId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.ServisSilebilmekIcin
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

        public Servis Get(Expression<Func<Servis, bool>> expression = null, params Expression<Func<Servis, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public int GetCount(Expression<Func<Servis, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public IEnumerable<Servis> List(Expression<Func<Servis, bool>> expression = null, params Expression<Func<Servis, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<Servis> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<Servis>
            {
                data = data.GetAllIncludeMultiple(y => y.Sube)
            };

            return entityPagedDataSource;
        }
    }
}
