using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Data.Concrete.EntityFramework
{
    public class EfOgrenciSinavKontrolData : EFEntityRepository<OgrenciSinavKontrol, EFContext>, IEfOgrenciSinavKontrolData
    {
        [LogAspect]
        public string UpdateWithNested(OgrenciSinavKontrol entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<OgrenciSinavKontrol>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;

                    if (entity.OgrenciSinavKontrolDersBilgiler != null && entity.OgrenciSinavKontrolDersBilgiler.Any())
                    {
                        foreach (var ogrenciSinavKontrolDersBilgi in entity.OgrenciSinavKontrolDersBilgiler)
                        {
                            context.Set<OgrenciSinavKontrolDersBilgi>().Attach(ogrenciSinavKontrolDersBilgi);
                            context.Entry(ogrenciSinavKontrolDersBilgi).State = EntityState.Modified;
                        }
                    }

                    if (entity.OgrenciSinavKontrolPuanTurPuanlar != null && entity.OgrenciSinavKontrolPuanTurPuanlar.Any())
                    {
                        foreach (var ogrenciSinavKontrolPuanTurPuan in entity.OgrenciSinavKontrolPuanTurPuanlar)
                        {
                            context.Set<OgrenciSinavKontrolPuanTurPuan>().Attach(ogrenciSinavKontrolPuanTurPuan);
                            context.Entry(ogrenciSinavKontrolPuanTurPuan).State = EntityState.Modified;
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
                returnMessage = ReturnExceptionMessage(entity, exception);
            }

            return returnMessage;
        }
    }
}
