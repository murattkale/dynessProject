using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfSinavData : IEntityRepository<Sinav>
    {
        string AddWithNestedLists(Sinav entity);

        string UpdateWithNestedLists(Sinav entity);
    }
}
