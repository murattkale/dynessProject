using Core.Entities.Dto;
using Entities.Concrete;
using System.Collections.Generic;
using System.Linq;

namespace WebUI.Models
{
    public class TopluOgrenciSinavKontrolViewModel
    {
        public List<OgrenciSinavKontrolViewModel> Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<List<OgrenciSinavKontrolViewModel>> OperationResult { get; set; }
    }
}