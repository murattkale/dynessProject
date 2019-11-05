using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;

namespace Data.Concrete.EntityFramework
{
    public class EfPersonelYetkiData : EFEntityRepository<PersonelYetki, EFContext>, IEfPersonelYetkiData
    {
    }
}
