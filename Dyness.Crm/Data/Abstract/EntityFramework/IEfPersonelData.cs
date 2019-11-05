using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfPersonelData : IEntityRepository<Personel>
    {
        string UpdateWithNestedLists(Personel entity);
    }
}
