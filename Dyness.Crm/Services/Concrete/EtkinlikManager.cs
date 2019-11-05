using Services.Abstract;
using System;
using System.Collections.Generic;
using Entities.Concrete;
using System.Linq.Expressions;
using Data.Abstract.EntityFramework;
using Core.Entities.Dto;
using System.Linq;
using Services.Validation;

namespace Services.Concrete
{
    public class EtkinlikManager : IEtkinlikService
    {
        IEfEtkinlikData data;
        IEfOgrenciSozlesmeData ogrenciSozlesmeData;

        public EtkinlikManager(IEfEtkinlikData data, IEfOgrenciSozlesmeData ogrenciSozlesmeData)
        {
            this.data = data;
            this.ogrenciSozlesmeData = ogrenciSozlesmeData;
        }

        public EntityOperationResult<Etkinlik> Add(Etkinlik entity)
        {
            var entityOperationResult = new EntityOperationResult<Etkinlik>(entity);

            var validationResults = EntityValidator<Etkinlik>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x => x.EtkinlikAd.ToLower() == entity.EtkinlikAd.ToLower());

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
                });

                return entityOperationResult;
            }

            entity.OlusturulmaTarihi = DateTime.Now;

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

        public EntityOperationResult<Etkinlik> Update(Etkinlik entity)
        {
            var entityOperationResult = new EntityOperationResult<Etkinlik>(entity);

            var validationResults = EntityValidator<Etkinlik>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

                return entityOperationResult;
            }

            var checkExistsCount = data.GetByWhereCaseCount(x =>
                x.EtkinlikAd.ToLower() == entity.EtkinlikAd.ToLower() &&
                x.EtkinlikId != entity.EtkinlikId);

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

        public EntityOperationResult<Etkinlik> DeleteById(int id)
        {
            var entityOperationResult = new EntityOperationResult<Etkinlik>();

            List<MessageInfo> validationResults = new List<MessageInfo>();

            var checkExistsCount = ogrenciSozlesmeData.GetByWhereCaseCount(x => x.EtkinlikId == id);

            if (checkExistsCount > 0)
            {
                entityOperationResult.MessageInfos.Add(new MessageInfo
                {
                    MessageInfoType = MessageInfoType.Error,
                    Message = Core.Properties.ServiceNoticesResources.EtkinligiSilebilmekIcinBagliSozlesme
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

        public int GetCount(Expression<Func<Etkinlik, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public Etkinlik Get(Expression<Func<Etkinlik, bool>> expression = null, params Expression<Func<Etkinlik, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        public IEnumerable<Etkinlik> List(Expression<Func<Etkinlik, bool>> expression = null, params Expression<Func<Etkinlik, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<Etkinlik> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<Etkinlik>
            {
                data = data.GetAllIncludeMultiple(x => x.SorumluPersonel)
            };

            return entityPagedDataSource;
        }
    }
}
