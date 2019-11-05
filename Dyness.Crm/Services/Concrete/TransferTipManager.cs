using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Entities.Concrete;
using Services.Abstract;
using Data.Abstract.EntityFramework;
using Core.Entities.Dto;
using System.Linq;
using Services.Validation;
using Core.Aspects.Postsharp.CacheAspects;

namespace Services.Concrete
{
    public class TransferTipManager :  ITransferTipService
    {
        IEfTransferTipData data;
        IEfHesapData hesapData;

        public TransferTipManager(IEfTransferTipData data,IEfHesapData hesapData)
        {
            this.data = data;
            this.hesapData = hesapData;
        }

        [CacheRemoveAspect]
        public EntityOperationResult<TransferTip> Add(TransferTip entity)
        {
            EntityOperationResult<TransferTip> entityOperationResult = new EntityOperationResult<TransferTip>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

          //  var validationResults = EntityValidator<TransferTip>.ValidateEntity(entity).ToList();
          //
          //  if (validationResults.Any())
          //  {
          //      entityOperationResult.MessageInfos = validationResults;
          //
          //      return entityOperationResult;
          //  }
          //
          //  var checkExistsCount = data.GetByWhereCaseCount(x => x.UlkeAd.ToLower() == entity.UlkeAd.ToLower());
          //
          //  if (checkExistsCount > 0)
          //  {
          //      entityOperationResult.MessageInfos.Add(new MessageInfo
          //      {
          //          MessageInfoType = MessageInfoType.Error,
          //          Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
          //      });
          //
          //      return entityOperationResult;
          //  }
          //
          //  var returnMessage = data.Add(entity);
          //
          //  if (!string.IsNullOrEmpty(returnMessage))
          //  {
          //      entityOperationResult.MessageInfos.Add(new MessageInfo
          //      {
          //          MessageInfoType = MessageInfoType.Error,
          //          Message = returnMessage
          //      });
          //
          //      return entityOperationResult;
          //  }
          //
          //  entityOperationResult.Status = true;
          //  entityOperationResult.MessageInfos.Add(new MessageInfo
          //  {
          //      MessageInfoType = MessageInfoType.Success,
          //      Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
          //  });

            return entityOperationResult;
        }

        [CacheRemoveAspect]
        public EntityOperationResult<TransferTip> Update(TransferTip entity)
        {
            EntityOperationResult<TransferTip> entityOperationResult = new EntityOperationResult<TransferTip>
            {
                Model = entity,
                MessageInfos = new List<MessageInfo>()
            };

         //   var validationResults = EntityValidator<TransferTip>.ValidateEntity(entity).ToList();
         //
         //   if (validationResults.Any())
         //   {
         //       entityOperationResult.MessageInfos = validationResults;
         //
         //       return entityOperationResult;
         //   }
         //
         //   var checkExistsCount = data.GetByWhereCaseCount(x =>
         //   x.UlkeAd.ToLower() == entity.UlkeAd.ToLower() &&
         //   x.UlkeId != entity.UlkeId);
         //
         //   if (checkExistsCount > 0)
         //   {
         //       entityOperationResult.MessageInfos.Add(new MessageInfo
         //       {
         //           MessageInfoType = MessageInfoType.Error,
         //           Message = Core.Properties.ServiceNoticesResources.VarolanKaydi
         //       });
         //
         //       return entityOperationResult;
         //   }
         //
         //   var returnMessage = data.Update(entity);
         //
         //   if (!string.IsNullOrEmpty(returnMessage))
         //   {
         //       entityOperationResult.MessageInfos.Add(new MessageInfo
         //       {
         //           MessageInfoType = MessageInfoType.Error,
         //           Message = returnMessage
         //       });
         //
         //       return entityOperationResult;
         //   }
         //
         //   entityOperationResult.Status = true;
         //   entityOperationResult.MessageInfos.Add(new MessageInfo
         //   {
         //       MessageInfoType = MessageInfoType.Success,
         //       Message = Core.Properties.ServiceNoticesResources.BasariylaGuncellendi
         //   });

            return entityOperationResult;
        }

        public EntityOperationResult<TransferTip> DeleteById(int id)
        {
            EntityOperationResult<TransferTip> entityOperationResult = new EntityOperationResult<TransferTip>
            {
                MessageInfos = new List<MessageInfo>()
            };

            List<MessageInfo> validationResults = new List<MessageInfo>();

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

        public int GetCount(Expression<Func<TransferTip, bool>> expression = null)
        {
            return data.GetByWhereCaseCount(expression);
        }

        public TransferTip Get(Expression<Func<TransferTip, bool>> expression = null, params Expression<Func<TransferTip, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultipleFirstOrDefault(expression, includes);
        }

        [CacheAspect]
        public IEnumerable<TransferTip> List(Expression<Func<TransferTip, bool>> expression = null, params Expression<Func<TransferTip, object>>[] includes)
        {
            return data.GetByWhereCaseIncludeMultiple(expression, includes);
        }

        public EntityPagedDataSource<TransferTip> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            var entityPagedDataSource = new EntityPagedDataSource<TransferTip>
            {
                data = data.GetAll()
            };

            return entityPagedDataSource;
        }
    }
}
