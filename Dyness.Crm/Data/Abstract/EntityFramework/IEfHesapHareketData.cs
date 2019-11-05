using Core.Data;
using Entities.Concrete;
using System.Collections.Generic;

namespace Data.Abstract.EntityFramework
{
    public interface IEfHesapHareketData : IEntityRepository<HesapHareket>
    {
        string AddUpdateLists(OgrenciSozlesme ogrenciSozlesme, List<HesapHareket> eklenecekler, List<HesapHareket> guncellenecekler);

        string UpdateWithOgrenciSozlesme(HesapHareket entity, OgrenciSozlesme ogrenciSozlesme);
    }
}
