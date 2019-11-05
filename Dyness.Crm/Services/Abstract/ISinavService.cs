using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface ISinavService : IServiceBase, IServiceModel<Sinav>
    {
        IEnumerable<Sinav> SinavListele(IEnumerable<Parameter> parameters);
    }
}
