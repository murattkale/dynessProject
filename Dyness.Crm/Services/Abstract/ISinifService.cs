using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface ISinifService : IServiceBase, IServiceModel<Sinif>
    {
        IEnumerable<Sinif> SinifListele(IEnumerable<Parameter> parameters);
    }
}
