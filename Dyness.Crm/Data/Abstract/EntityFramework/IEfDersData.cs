using Core.Data;
using Entities.Concrete;
using System.Collections.Generic;

namespace Data.Abstract.EntityFramework
{
    public interface IEfDersData : IEntityRepository<Ders>
    {
        string AddWithNestedLists(Ders entity, List<BransDers> bransDersler);

        string UpdateWithNestedLists(Ders entity, List<BransDers> bransDersler);
    }
}
