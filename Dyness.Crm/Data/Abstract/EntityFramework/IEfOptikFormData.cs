using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfOptikFormData : IEntityRepository<OptikForm>
    {
        string AddWithNestedLists(OptikForm entity);

        string UpdateWithNestedLists(OptikForm entity);
    }
}
