using Core.Entities.Dto;
using WebUI.Models.Abstract;

namespace WebUI.Models
{
    public class SmsHesapHareketListeleViewModel : ViewModelListele<SmsHesapHareketDto>
    {
        public string IlkTarih { get; set; }

        public string SonTarih { get; set; }
    }
}