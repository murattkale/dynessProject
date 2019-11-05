using Core.Services;
using Entities.Concrete;

namespace Services.Abstract
{
    public interface IKonuService : IServiceBase, IServiceModel<Konu>
    {
    }
}
