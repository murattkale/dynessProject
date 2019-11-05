using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface IOgrenciSozlesmeService : IServiceBase, IServiceModel<OgrenciSozlesme>
    {
        EntityOperationResult<OgrenciSozlesme> UpdateOdemeBilgi(OgrenciSozlesme entity, bool odemelerSilinsinMi);

        EntityOperationResult<List<OgrenciSozlesme>> UpdateList(List<OgrenciSozlesme> entities);

        EntityPagedDataSource<OgrenciSozlesmeDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);
    }
}
