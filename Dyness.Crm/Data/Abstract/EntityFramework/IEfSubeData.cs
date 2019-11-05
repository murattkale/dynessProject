using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfSubeData : IEntityRepository<Sube>
    {
        string UpdateWithNested(Sube entity);

        string DeleteWithNested(Sube entity);

    }
}
