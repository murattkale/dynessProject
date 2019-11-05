using Core.Entities.Dto;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Models
{
    public class DuyuruDuzenleViewModel
    {
        public string Command { get; set; }

        public List<SelectListItem> KullaniciGrupSelectListItemList { get; set; }

        public SelectList KullaniciGrupSelectList { get; set; }

        public Duyuru Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Duyuru> OperationResult { get; set; }
    }
}