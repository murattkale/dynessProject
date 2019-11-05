using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface IOgrenciSinavKontrolPuanTurPuanService : IServiceBase, IServiceModel<OgrenciSinavKontrolPuanTurPuan>
    {
        List<OgrenciSinavKontrolPuanTurPuanDto> ListDto(int sinavId);

        void UpdateDto(OgrenciSinavKontrolPuanTurPuanDto dto);
    }
}
