using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfSmsData : IEntityRepository<Sms>
    {
        string AddWithSmsHesapHareket(Sms entity);
    }
}
