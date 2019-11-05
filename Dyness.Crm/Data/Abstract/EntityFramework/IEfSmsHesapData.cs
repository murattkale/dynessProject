using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfSmsHesapData : IEntityRepository<SmsHesap>
    {
        string UpdateWithNestedLists(SmsHesap entity);
    }
}
