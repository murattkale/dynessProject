using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface ISezonService : IServiceBase, IServiceModel<Sezon>
    {
        IEnumerable<Sezon> SezonListele(IEnumerable<Parameter> parameters);
    }
}
