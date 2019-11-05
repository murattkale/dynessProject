using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;

namespace Services.Abstract
{
    public interface IKullaniciService : IServiceBase, IServiceModel<Kullanici>
    {
        EntityOperationResult<Kullanici> Giris(string kullaniciAd, string sifre);
    }
}
