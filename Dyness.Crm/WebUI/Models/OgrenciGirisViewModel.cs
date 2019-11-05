using Core.Entities.Dto;
using Core.Properties;
using Entities.Concrete;
using System.Linq;
using WebUI.Models.Abstract;
using System.ComponentModel.DataAnnotations;
using System;

namespace WebUI.Models
{
    public class OgrenciGirisViewModel : IViewModelDuzenle<Ogrenci>
    {
        public string Command { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(11, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string OgrenciTcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "TcNo")]
        [MaxLength(11, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(11, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string VeliTcKimlikNo { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        [DataType(DataType.Date)]
        public DateTime? OgrenciDogumTarihi { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "DogumTarihi")]
        public string OgrenciDogumTarihiFormatted { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sifre")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(4, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string OgrenciSifre { get; set; }

        [Display(ResourceType = typeof(FieldNameResources), Name = "Sifre")]
        [MaxLength(20, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMaxLength")]
        [MinLength(4, ErrorMessageResourceType = typeof(Core.Properties.Resources), ErrorMessageResourceName = "DataAnnotationsMinLength")]
        public string VeliSifre { get; set; }

        public Ogrenci Model { get; set; }

        public bool MessageExists => OperationResult != null && OperationResult.MessageInfos != null && OperationResult.MessageInfos.Count() > 0;

        public EntityOperationResult<Ogrenci> OperationResult { get; set; }
    }
}