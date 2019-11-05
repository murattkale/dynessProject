using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Data.Concrete.EntityFramework
{
    public class EfHesapHareketData : EFEntityRepository<HesapHareket, EFContext>, IEfHesapHareketData
    {
        [LogAspect]
        public string AddUpdateLists(OgrenciSozlesme ogrenciSozlesme, List<HesapHareket> eklenecekler, List<HesapHareket> guncellenecekler)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<OgrenciSozlesme>().Attach(ogrenciSozlesme);
                    context.Entry(ogrenciSozlesme).State = EntityState.Modified;

                    if(eklenecekler != null && eklenecekler.Count > 0)
                    {
                        foreach (var eklenecek in eklenecekler)
                        {
                            context.Set<HesapHareket>().Add(eklenecek);
                        }
                    }

                    if (guncellenecekler != null && guncellenecekler.Count > 0)
                    {
                        foreach (var guncellenecek in guncellenecekler)
                        {
                            context.Set<HesapHareket>().Attach(guncellenecek);
                            context.Entry(guncellenecek).State = EntityState.Modified;
                        }
                    }

                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                returnMessage = ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                var entity = new HesapHareket();

                returnMessage = ReturnExceptionMessage(entity, exception);
            }

            return returnMessage;
        }

        [LogAspect]
        public string UpdateWithOgrenciSozlesme(HesapHareket entity, OgrenciSozlesme ogrenciSozlesme)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<OgrenciSozlesme>().Attach(ogrenciSozlesme);
                    context.Entry(ogrenciSozlesme).State = EntityState.Modified;

                    context.Set<HesapHareket>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;
                  

                    context.SaveChanges();
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
    }
}
