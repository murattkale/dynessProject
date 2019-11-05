using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;

namespace Data.Concrete.EntityFramework
{
    public class EfSinavTurData : EFEntityRepository<SinavTur, EFContext>, IEfSinavTurData
    {
        [LogAspect]
        public string AddWithNestedLists(SinavTur entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    var sinavTurDersKatSayilar = new List<SinavTurDersKatSayi>();

                    if (entity.SinavTurDersKatSayilar != null)
                    {
                        foreach (var sinavTurDersKatSayi in entity.SinavTurDersKatSayilar)
                        {
                            if ((sinavTurDersKatSayi.KatSayi ?? 0) == 0)
                                continue;

                            sinavTurDersKatSayi.DersGrup = null;
                            sinavTurDersKatSayi.PuanTur = null;
                            sinavTurDersKatSayi.SinavTur = entity;

                            sinavTurDersKatSayilar.Add(sinavTurDersKatSayi);
                        }
                    }

                    var sinavTurDersler = new List<SinavTurDers>();

                    if (entity.SinavTurDersler != null)
                    {
                        foreach (var sinavTurDers in entity.SinavTurDersler)
                        {
                            if ((sinavTurDers.SoruSayi ?? 0) == 0 || (sinavTurDers.Sira ?? 0) == 0)
                                continue;

                            sinavTurDers.Ders = null;
                            sinavTurDers.SinavTur = entity;

                            sinavTurDersler.Add(sinavTurDers);
                        }
                    }

                    if (sinavTurDersKatSayilar != null && sinavTurDersKatSayilar.Any())
                        entity.SinavTurDersKatSayilar = sinavTurDersKatSayilar;
                    else
                        entity.SinavTurDersKatSayilar = null;

                    if (sinavTurDersler != null && sinavTurDersler.Any())
                        entity.SinavTurDersler = sinavTurDersler;
                    else
                        entity.SinavTurDersler = null;

                    context.Set<SinavTur>().AddOrUpdate(entity);

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
        public string UpdateWithNestedLists(SinavTur entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    if (entity.SinavTurDersKatSayilar != null && entity.SinavTurDersKatSayilar.Any())
                    {
                        foreach (var sinavTurDersKatSayi in entity.SinavTurDersKatSayilar)
                        {
                            sinavTurDersKatSayi.DersGrup = null;
                            sinavTurDersKatSayi.PuanTur = null;

                            if ((sinavTurDersKatSayi.KatSayi ?? 0) == 0 && sinavTurDersKatSayi.SinavTurDersKatSayiId > 0)
                                context.Entry(sinavTurDersKatSayi).State = System.Data.Entity.EntityState.Deleted;
                            else if ((sinavTurDersKatSayi.KatSayi ?? 0) > 0)
                                context.Set<SinavTurDersKatSayi>().AddOrUpdate(sinavTurDersKatSayi);
                        }
                    }

                    if (entity.SinavTurDersler != null && entity.SinavTurDersler.Any())
                    {
                        foreach (var sinavTurDers in entity.SinavTurDersler)
                        {
                            sinavTurDers.Ders = null;

                            if (((sinavTurDers.SoruSayi ?? 0) == 0 || (sinavTurDers.Sira ?? 0) == 0) && sinavTurDers.SinavTurDersId > 0)
                                context.Entry(sinavTurDers).State = System.Data.Entity.EntityState.Deleted;
                            else if (sinavTurDers.SoruSayi != null && sinavTurDers.SoruSayi > 0 && sinavTurDers.Sira != null && sinavTurDers.Sira > 0)
                                context.Set<SinavTurDers>().AddOrUpdate(sinavTurDers);
                        }
                    }

                    entity.SinavTurDersKatSayilar = null;
                    entity.SinavTurDersler = null;

                    context.Set<SinavTur>().AddOrUpdate(entity);

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
