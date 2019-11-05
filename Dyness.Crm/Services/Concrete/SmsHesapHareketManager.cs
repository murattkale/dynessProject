using Core.CrossCuttingConcerns.Security;
using Core.Entities.Dto;
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
    public class SmsHesapHareketManager : ISmsHesapHareketService
    {
        IEfSmsHesapHareketData data;
        IDpSmsHesapHareketData dpData;

        public SmsHesapHareketManager(IEfSmsHesapHareketData data, IDpSmsHesapHareketData dpData)
        {
            this.data = data;
            this.dpData = dpData;
        }

        public EntityOperationResult<SmsHesapHareket> Add(SmsHesapHareket entity)
        {
            var entityOperationResult = new EntityOperationResult<SmsHesapHareket>(entity);

            var validationResults = EntityValidator<SmsHesapHareket>.ValidateEntity(entity).ToList();

            if (validationResults.Any())
            {
                entityOperationResult.MessageInfos = validationResults;

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

        public EntityOperationResult<SmsHesapHareket> Update(SmsHesapHareket entity)
        {
            throw new NotImplementedException();
        }

        public EntityOperationResult<SmsHesapHareket> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public int GetCount(Expression<Func<SmsHesapHareket, bool>> expression = null)
        {
            throw new NotImplementedException();
        }

        public SmsHesapHareket Get(Expression<Func<SmsHesapHareket, bool>> expression = null, params Expression<Func<SmsHesapHareket, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SmsHesapHareket> List(Expression<Func<SmsHesapHareket, bool>> expression = null, params Expression<Func<SmsHesapHareket, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<SmsHesapHareket> EntityPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            throw new NotImplementedException();
        }

        public EntityPagedDataSource<SmsHesapHareketDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null)
        {
            if (string.IsNullOrEmpty(filter.SortColumnName))
                filter.SortDirection = "desc";

            var newParameters = parameters == null ? new List<Parameter>() : parameters.ToList();

            newParameters.Add(new Parameter("PersonelId", Identity.PersonelId));

            return dpData.GetPagedEntities("SmsHesapHareket_Listele", filter, newParameters);
        }
    }
}
