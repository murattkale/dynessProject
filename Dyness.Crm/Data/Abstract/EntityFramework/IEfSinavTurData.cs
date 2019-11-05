using Core.Data;
using Entities.Concrete;
using System.Collections.Generic;

namespace Data.Abstract.EntityFramework
{
    public interface IEfSinavTurData : IEntityRepository<SinavTur>
    {
        string AddWithNestedLists(SinavTur entity);

        string UpdateWithNestedLists(SinavTur entity);
    }
}
