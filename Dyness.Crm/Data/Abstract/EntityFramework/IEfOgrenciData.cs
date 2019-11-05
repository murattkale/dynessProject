using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfOgrenciData : IEntityRepository<Ogrenci>
    {
        string UpdateWithNested(Ogrenci entity);
    }
}
