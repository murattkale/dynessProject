using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface IOgrenciSinavKontrolService : IServiceBase, IServiceModel<OgrenciSinavKontrol>
    {
        EntityPagedDataSource<OgrenciSinavKontrolDto> DtoPagedDataSource(EntityPagedDataSourceFilter filter, IEnumerable<Parameter> parameters = null);
    }
}
