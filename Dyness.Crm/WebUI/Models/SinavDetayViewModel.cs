using Core.Entities.Dto;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class SinavDetayViewModel : ViewModelListele<OgrenciSinavKontrolDto>
    {
        public List<Tuple<Konu, int>> Konular { get; set; }

        public new Sinav Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Sinav> OperationResult { get; set; }

        public OgrenciSinavKontrolListeleViewModel ListeleModel { get; set; }
    }
}