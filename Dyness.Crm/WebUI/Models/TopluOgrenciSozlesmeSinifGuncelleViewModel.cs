using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Core.Entities.Dto;
using Entities.Concrete;

namespace WebUI.Models
{
    public struct OgrenciSozlesmeSinif
    {
        public int SinifId { get; set; }

        public int[] OgrenciSozlesmeIdler { get; set; }

        public List<SelectListItem> OgrenciSozlesmeSelectList { get; set; }
    }

    public class TopluOgrenciSozlesmeSinifGuncelleViewModel
    {
        public int SubeId { get; set; }

        public List<Sinif> Siniflar { get; set; }

        public List<SelectListItem> SubeSelectList { get; set; }

        public List<OgrenciSozlesmeSinif> OgrenciSozlesmeSiniflar { get; set; }

        public string Command { get; set; }

        public bool MessageExists => OperationResult?.MessageInfos != null && OperationResult.MessageInfos.Any();

        public EntityOperationResult<List<OgrenciSozlesme>> OperationResult { get; set; }
    }
}