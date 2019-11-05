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
    public class EfPersonelData : EFEntityRepository<Personel, EFContext>, IEfPersonelData
    {
        [LogAspect]
        public string UpdateWithNestedLists(Personel entity)
        {
            var returnMessage = string.Empty;
            var updatePersonelSubeDersler = entity.PersonelSubeDersler;
            var updatePersonelSubeYetkiler = entity.PersonelSubeYetkiler;
            var personelSubeUcretler = entity.PersonelSubeUcretler;

            try
            {
                using (var context = new EFContext())
                {
                    entity.PersonelSubeYetkiler = null;
                    entity.PersonelSubeDersler = null;
                    entity.PersonelSubeUcretler = null;

                    context.Set<Personel>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;

                    #region Personel Şube Dersler

                    var personelSubeDersler = context.Set<PersonelSubeDers>().Where(x => x.PersonelId == entity.PersonelId).ToList();

                    if (personelSubeDersler != null && personelSubeDersler.Count > 0)
                    {
                        var willRemovedPersonelSubeDersler = new List<PersonelSubeDers>();

                        foreach (var personelSubeDers in personelSubeDersler)
                        {
                            var existsCount = updatePersonelSubeDersler == null
                                ? 0
                                : updatePersonelSubeDersler.Count(x => x.SubeId == personelSubeDers.SubeId);

                            if (existsCount > 0)
                                continue;

                            willRemovedPersonelSubeDersler.Add(personelSubeDers);
                        }

                        if (willRemovedPersonelSubeDersler.Count > 0)
                        {
                            for (int i = 0; i < willRemovedPersonelSubeDersler.Count; i++)
                            {
                                context.Set<PersonelSubeDers>().Remove(willRemovedPersonelSubeDersler[i]);
                            }
                        }
                    }

                    if (updatePersonelSubeDersler != null && updatePersonelSubeDersler.Count > 0)
                    {
                        foreach (var updatePersonelSubeDers in updatePersonelSubeDersler)
                        {
                            var existsCount = personelSubeDersler.Count(x => x.SubeId == updatePersonelSubeDers.SubeId);

                            if (existsCount > 0)
                                continue;

                            context.Set<PersonelSubeDers>().Add(updatePersonelSubeDers);
                        }
                    }

                    #endregion

                    #region Personel Şube Yetkiler

                    var personelSubeYetkiler = context.Set<PersonelSubeYetki>().Where(x => x.PersonelId == entity.PersonelId).ToList();

                    if (personelSubeYetkiler != null && personelSubeYetkiler.Count > 0)
                    {
                        var willRemovedPersonelSubeYetkiler = new List<PersonelSubeYetki>();

                        foreach (var personelSubeYetki in personelSubeYetkiler)
                        {
                            var existsCount = updatePersonelSubeYetkiler == null
                                ? 0
                                : updatePersonelSubeYetkiler.Count(x => x.SubeId == personelSubeYetki.SubeId);

                            if (existsCount > 0)
                                continue;

                            willRemovedPersonelSubeYetkiler.Add(personelSubeYetki);
                        }

                        if (willRemovedPersonelSubeYetkiler.Count > 0)
                        {
                            for (int i = 0; i < willRemovedPersonelSubeYetkiler.Count; i++)
                            {
                                context.Set<PersonelSubeYetki>().Remove(willRemovedPersonelSubeYetkiler[i]);
                            }
                        }
                    }

                    if (updatePersonelSubeYetkiler != null && updatePersonelSubeYetkiler.Count > 0)
                    {
                        foreach (var updatePersonelSubeYetki in updatePersonelSubeYetkiler)
                        {
                            var existsCount = personelSubeYetkiler.Count(x => x.SubeId == updatePersonelSubeYetki.SubeId);

                            if (existsCount > 0)
                                continue;

                            context.Set<PersonelSubeYetki>().Add(updatePersonelSubeYetki);
                        }
                    }

                    #endregion

                    if (personelSubeUcretler != null && personelSubeUcretler.Count > 0)
                    {
                        foreach (var item in personelSubeUcretler)
                        {
                            if (item.Silinecek)
                            {
                                var dbSet = context.Set<PersonelSubeUcret>();
                                var removedItem = dbSet.Find(item.PersonelSubeUcretId);

                                if (removedItem != null)
                                    context.Set<PersonelSubeUcret>().Remove(removedItem);
                            }
                            else if (item.PersonelSubeUcretId == 0)
                            {
                                var personelSubeUcret = new PersonelSubeUcret
                                {
                                    PersonelId = entity.PersonelId,
                                    SubeId = item.SubeId,
                                    Ucret = item.Ucret
                                };

                                context.Set<PersonelSubeUcret>().Add(personelSubeUcret);
                            }
                            else
                            {
                                context.Set<PersonelSubeUcret>().Attach(item);
                                context.Entry(item).State = EntityState.Modified;
                            }
                        }
                    }

                    if (entity.Kullanici != null)
                    {
                        var kullaniciVar = context.Kullanicilar.Count(x => x.PersonelId == entity.Kullanici.PersonelId) > 0;

                        if (kullaniciVar)
                        {
                            context.Set<Kullanici>().Attach(entity.Kullanici);
                            context.Entry(entity.Kullanici).State = EntityState.Modified;
                        }
                        else
                        {
                            context.Set<Kullanici>().AddOrUpdate(entity.Kullanici);
                        }
                    }

                    context.Set<Hesap>().Attach(entity.Hesap);
                    context.Entry(entity.Hesap).State = EntityState.Modified;

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
    }
}
