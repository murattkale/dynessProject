using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfOgrenciSinavKontrolData : IEntityRepository<OgrenciSinavKontrol>
    {
        string UpdateWithNested(OgrenciSinavKontrol entity);
    }
}
