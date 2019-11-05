using Core.Entities.Dto;
using Entities.Concrete;
using System.Linq;

namespace WebUI.Models
{
    public class OgrenciSinavKontrolViewModel
    {
        public int OgrenciAdet { get; set; }

        public int SubeOgrenciAdet { get; set; }

        public int SinifOgrenciAdet { get; set; }

        public bool Yazdir { get; set; }

        public OgrenciSinavKontrol Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<OgrenciSinavKontrol> OperationResult { get; set; }
    }
}