using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfBankaHesapData : IEntityRepository<BankaHesap>
    {
        string UpdateWithNested(BankaHesap entity);
    }
}
