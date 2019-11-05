using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfVideoData : IEntityRepository<Video>
    {
        string AddWithNested(Video entity);

        string UpdateWithNested(Video entity);
    }
}
