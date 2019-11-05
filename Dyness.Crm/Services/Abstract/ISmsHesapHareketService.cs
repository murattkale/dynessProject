using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface ISmsHesapHareketService : IServiceBase, IServiceModel<SmsHesapHareket>
    {
        EntityPagedDataSource<SmsHesapHareketDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);
    }
}
