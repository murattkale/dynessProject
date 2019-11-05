using Core.Entities.Dto;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class OgrenciHesapListeleViewModel : ViewModelListele<OgrenciHesapDto>
    {
        public bool VadesiGecen { get; set; }

        public bool EtkinMi { get; set; }
    }
}