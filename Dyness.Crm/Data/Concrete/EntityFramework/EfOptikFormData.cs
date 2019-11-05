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
    public class EfOptikFormData : EFEntityRepository<OptikForm, EFContext>, IEfOptikFormData
    {
        [LogAspect]
        public string AddWithNestedLists(OptikForm entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    var optikFormDersGrupBilgiler = new List<OptikFormDersGrupBilgi>();

                    if (entity.OptikFormDersGrupBilgiler != null)
                    {
                        foreach (var optikFormDersGrupBilgi in entity.OptikFormDersGrupBilgiler)
                        {
                            if ((optikFormDersGrupBilgi.DersGrupBasla ?? 0) == 0 || (optikFormDersGrupBilgi.DersGrupAdet ?? 0) == 0)
                                continue;

                            optikFormDersGrupBilgi.DersGrup = null;
                            optikFormDersGrupBilgiler.Add(optikFormDersGrupBilgi);
                        }
                    }

                    if (optikFormDersGrupBilgiler != null && optikFormDersGrupBilgiler.Any())
                        entity.OptikFormDersGrupBilgiler = optikFormDersGrupBilgiler;
                    else
                        entity.OptikFormDersGrupBilgiler = null;

                    context.Set<OptikForm>().AddOrUpdate(entity);

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
        public string UpdateWithNestedLists(OptikForm entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    if (entity.OptikFormDersGrupBilgiler != null && entity.OptikFormDersGrupBilgiler.Any())
                    {
                        foreach (var optikFormDersGrupBilgi in entity.OptikFormDersGrupBilgiler)
                        {
                            optikFormDersGrupBilgi.DersGrup = null;

                            if (((optikFormDersGrupBilgi.DersGrupBasla ?? 0) == 0 || (optikFormDersGrupBilgi.DersGrupAdet ?? 0) == 0) && optikFormDersGrupBilgi.OptikFormDersGrupBilgiId > 0)
                                context.Entry(optikFormDersGrupBilgi).State = System.Data.Entity.EntityState.Deleted;
                            else if (optikFormDersGrupBilgi.DersGrupBasla != null && optikFormDersGrupBilgi.DersGrupBasla > 0 && optikFormDersGrupBilgi.DersGrupAdet != null && optikFormDersGrupBilgi.DersGrupAdet > 0)
                                context.Set<OptikFormDersGrupBilgi>().AddOrUpdate(optikFormDersGrupBilgi);
                        }

                        context.Set<OptikForm>().AddOrUpdate(entity);
                        context.SaveChanges();
                    }
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
