using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfSinavKitapcikData : IEntityRepository<SinavKitapcik>
    {
        string UpdateWithNested(SinavKitapcik entity);
    }
}
