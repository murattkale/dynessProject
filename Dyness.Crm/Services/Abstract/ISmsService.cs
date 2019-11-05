using Core.Entities.Dto;
using Core.Services;
using Entities.Concrete;
using System.Collections.Generic;

namespace Services.Abstract
{
    public interface ISmsService : IServiceBase, IServiceModel<Sms>
    {
        // 0 öğrenci, 1 personel
        IEnumerable<SmsTelefonBilgiDto> SmsTelefonBilgiListele(int tip, IEnumerable<Parameter> parameters);
    }
}
