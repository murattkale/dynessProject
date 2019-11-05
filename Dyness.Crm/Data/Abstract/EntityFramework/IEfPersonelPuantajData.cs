using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
   public interface IEfPersonelPuantajData : IEntityRepository<PersonelPuantaj>
    {
        string UpdateWithNestedLists(PersonelPuantaj entity);
    }
}
