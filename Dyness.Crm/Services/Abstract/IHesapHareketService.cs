using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface IHesapHareketService : IServiceBase, IServiceModel<HesapHareket>
    {
        EntityOperationResult<HesapHareket> AddUpdateLists(OgrenciSozlesme ogrenciSozlesme, List<HesapHareket> eklenecekler, List<HesapHareket> guncellenecekler);

        EntityOperationResult<HesapHareket> UpdateWithOgrenciSozlesme(HesapHareket entity, OgrenciSozlesme ogrenciSozlesme);

        int GetMinimumYear();

        int GetMaximumYear();

        EntityPagedDataSource<HesapHareketDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);
    }
}
