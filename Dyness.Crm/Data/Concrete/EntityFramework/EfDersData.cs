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
    public class EfDersData : EFEntityRepository<Ders, EFContext>, IEfDersData
    {
        [LogAspect]
        public string AddWithNestedLists(Ders entity, List<BransDers> bransDersler)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    if (entity.BransDersler != null)
                    {
                        foreach (var bransDers in entity.BransDersler)
                        {
                            context.Set<BransDers>().AddOrUpdate(bransDers);
                        }
                    }

                    context.Set<Ders>().AddOrUpdate(entity);

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
        public string UpdateWithNestedLists(Ders entity, List<BransDers> bransDersler)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    if (bransDersler != null && bransDersler.Any())
                    {
                        foreach (var bransDers in bransDersler)
                        {
                            if (bransDers.Silinecek)
                                context.Entry(bransDers).State = System.Data.Entity.EntityState.Deleted;
                            else
                                context.Set<BransDers>().AddOrUpdate(bransDers);
                        }
                    }

                    context.Set<Ders>().AddOrUpdate(entity);

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
