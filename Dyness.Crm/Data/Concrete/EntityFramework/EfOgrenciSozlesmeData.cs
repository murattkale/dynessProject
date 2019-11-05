using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;

namespace Data.Concrete.EntityFramework
{
    public class EfOgrenciSozlesmeData : EFEntityRepository<OgrenciSozlesme, EFContext>, IEfOgrenciSozlesmeData
    {
        [LogAspect]
        public string AddWithNestedLists(OgrenciSozlesme entity, Hesap subeHesapEntity)
        {
            string returnMessage;

            try
            {
                using (var context = new EFContext())
                {
                    var hesapHareketler = entity.Ogrenci.Hesap.HesapHareketler;

                    if (entity.OgrenciId > 0)
                    {
                        var ogrenci = entity.Ogrenci;
                        entity.Ogrenci = null;

                        context.Set<Ogrenci>().Attach(ogrenci);
                        context.Entry(ogrenci).State = EntityState.Modified;

                        context.Set<Hesap>().Attach(ogrenci.Hesap);
                        context.Entry(ogrenci.Hesap).State = EntityState.Modified;

                        context.Set<OgrenciYakiniIletisim>().Attach(ogrenci.AnneOgrenciYakiniIletisim);
                        context.Entry(ogrenci.AnneOgrenciYakiniIletisim).State = EntityState.Modified;

                        context.Set<OgrenciYakiniIletisim>().Attach(ogrenci.BabaOgrenciYakiniIletisim);
                        context.Entry(ogrenci.BabaOgrenciYakiniIletisim).State = EntityState.Modified;

                        context.Set<OgrenciYakiniIletisim>().Attach(ogrenci.YakiniOgrenciYakiniIletisim);
                        context.Entry(ogrenci.YakiniOgrenciYakiniIletisim).State = EntityState.Modified;
                    }

                    context.Set<OgrenciSozlesme>().Add(entity);

                    if (hesapHareketler != null && hesapHareketler.Count > 0)
                    {
                        foreach (var itehesapHareket in hesapHareketler)
                        {
                            context.Set<HesapHareket>().Add(itehesapHareket);
                        }
                    }

                    if (entity.OgrenciSozlesmeDersSecimler != null && entity.OgrenciSozlesmeDersSecimler.Count > 0)
                    {
                        foreach (var ogrenciSozlesmeDersSecim in entity.OgrenciSozlesmeDersSecimler)
                        {
                            context.Set<OgrenciSozlesmeDersSecim>().Add(ogrenciSozlesmeDersSecim);
                        }
                    }

                    if (subeHesapEntity != null)
                    {
                        context.Set<Hesap>().Attach(subeHesapEntity);
                        context.Entry(subeHesapEntity).State = EntityState.Modified;
                    }

                    context.SaveChanges();

                    returnMessage = string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                returnMessage = ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                returnMessage = ReturnExceptionMessage(entity, exception);
            }

            return returnMessage;
        }

        [LogAspect]
        public string UpdatedWithNestedLists(
            OgrenciSozlesme entity,
            List<OgrenciSozlesmeHesapHareket> silinecekler,
            List<OgrenciSozlesmeHesapHareket> eklenecekler)
        {
            string returnMessage;

            try
            {
                using (var context = new EFContext())
                {
                    foreach (var t in silinecekler)
                    {
                        var hesapHareket = t.HesapHareket;

                        context.Set<OgrenciSozlesmeHesapHareket>().Attach(t);
                        context.Entry(t).State = EntityState.Deleted;

                        context.Set<HesapHareket>().Attach(hesapHareket);
                        context.Entry(hesapHareket).State = EntityState.Deleted;
                    }

                    foreach (var t in eklenecekler)
                    {
                        context.Set<OgrenciSozlesmeHesapHareket>().AddOrUpdate(t);
                    }

                    context.Set<OgrenciSozlesmeOdemeBilgi>().AddOrUpdate(entity.OgrenciSozlesmeOdemeBilgi);
                    context.Set<OgrenciSozlesme>().AddOrUpdate(entity);
                    context.SaveChanges();

                    returnMessage = string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                returnMessage = ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                returnMessage = ReturnExceptionMessage(entity, exception);
            }

            return returnMessage;
        }

        [LogAspect]
        public string UpdateList(List<OgrenciSozlesme> entities)
        {
            string returnMessage;

            try
            {
                using (var context = new EFContext())
                {
                    foreach (var item in entities)
                    {
                        context.Set<OgrenciSozlesme>().Attach(item);
                        context.Entry(item).State = EntityState.Modified;
                    }

                    context.SaveChanges();

                    returnMessage = string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                returnMessage = ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                returnMessage = ReturnExceptionMessage(null, exception);
            }

            return returnMessage;
        }
    }
}
