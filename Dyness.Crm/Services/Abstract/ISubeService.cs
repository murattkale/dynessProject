using Core.Services;
using Entities.Concrete;

namespace Services.Abstract
{
    public interface ISubeService : IServiceBase, IServiceModel<Sube>
    {
        string GetSonSubeKod();
    }
}
