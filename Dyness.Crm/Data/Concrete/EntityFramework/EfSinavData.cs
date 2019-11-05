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
using System.Linq;

namespace Data.Concrete.EntityFramework
{
    public class EfSinavData : EFEntityRepository<Sinav, EFContext>, IEfSinavData
    {
        [LogAspect]
        public string AddWithNestedLists(Sinav entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<Sinav>().AddOrUpdate(entity);

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

        [LogAspect]
        public string UpdateWithNestedLists(Sinav entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    if (entity.SinavKitapciklar != null && entity.SinavKitapciklar.Any())
                    {
                        foreach (var sinavKitapcik in entity.SinavKitapciklar)
                        {
                            if (sinavKitapcik.SinavKitapcikId == 0 && string.IsNullOrEmpty(sinavKitapcik.Baslik))
                                continue;

                            if (string.IsNullOrEmpty(sinavKitapcik.Baslik) && sinavKitapcik.SinavKitapcikId > 0)
                            {
                                context.Entry(sinavKitapcik).State = System.Data.Entity.EntityState.Deleted;
                            }
                            else if (!string.IsNullOrEmpty(sinavKitapcik.Baslik) && sinavKitapcik.SinavKitapcikId == 0)
                            {
                                context.Set<SinavKitapcik>().Attach(sinavKitapcik);
                                context.Entry(entity).State = EntityState.Added;
                            }
                        }
                    }

                    if (entity.SinavSubeler != null && entity.SinavSubeler.Any())
                    {
                        var updateSinavSubeYetkiler = entity.SinavSubeler;
                        entity.SinavSubeler = null;

                        #region Personel Şube Yetkiler

                        var sinavSubeYetkiler = context.Set<SinavSube>().Where(x => x.SinavId == entity.SinavId).ToList();

                        if (sinavSubeYetkiler != null && sinavSubeYetkiler.Count > 0)
                        {
                            var willRemovedSinavSubeYetkiler = new List<SinavSube>();

                            foreach (var sinavSubeYetki in sinavSubeYetkiler)
                            {
                                var existsCount = updateSinavSubeYetkiler == null
                                    ? 0
                                    : updateSinavSubeYetkiler.Count(x => x.SubeId == sinavSubeYetki.SubeId);

                                if (existsCount > 0)
                                    continue;

                                willRemovedSinavSubeYetkiler.Add(sinavSubeYetki);
                            }

                            if (willRemovedSinavSubeYetkiler.Count > 0)
                            {
                                for (int i = 0; i < willRemovedSinavSubeYetkiler.Count; i++)
                                {
                                    context.Set<SinavSube>().Remove(willRemovedSinavSubeYetkiler[i]);
                                }
                            }
                        }

                        if (updateSinavSubeYetkiler != null && updateSinavSubeYetkiler.Count > 0)
                        {
                            foreach (var updateSinavSubeYetki in updateSinavSubeYetkiler)
                            {
                                var existsCount = sinavSubeYetkiler.Count(x => x.SubeId == updateSinavSubeYetki.SubeId);

                                if (existsCount > 0)
                                    continue;

                                updateSinavSubeYetki.SinavId = updateSinavSubeYetki.Sinav.SinavId;
                                updateSinavSubeYetki.Sinav = null;

                                context.Set<SinavSube>().Add(updateSinavSubeYetki);
                            }
                        }

                        #endregion
                    }

                    entity.SinavKitapciklar = null;

                    context.Set<Sinav>().Attach(entity);
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
