using System.Collections.Generic;
using Core.Data;
using Entities.Concrete;

namespace Data.Abstract.EntityFramework
{
    public interface IEfOgrenciSozlesmeData : IEntityRepository<OgrenciSozlesme>
    {
        string AddWithNestedLists(OgrenciSozlesme entity, Hesap subeHesapEntity);

        string UpdatedWithNestedLists(OgrenciSozlesme entity, List<OgrenciSozlesmeHesapHareket> silinecekler, List<OgrenciSozlesmeHesapHareket> eklenecekler);

        string UpdateList(List<OgrenciSozlesme> entities);
    }
}
