using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface IHesapService : IServiceBase, IServiceModel<Hesap>
    {
        EntityPagedDataSource<HesapDto> HesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);

        EntityPagedDataSource<OgrenciHesapDto> OgrenciHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);

        EntityPagedDataSource<HesapDto> PersonelHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);

        EntityPagedDataSource<HesapDto> SubeHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);

        EntityPagedDataSource<HesapDto> KasaBankaHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);

        EntityPagedDataSource<HesapDto> GiderHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);

        EntityPagedDataSource<HesapDto> GelirHesapDtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);
    }
}
