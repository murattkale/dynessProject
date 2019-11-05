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
    public class EfSubeData : EFEntityRepository<Sube, EFContext>, IEfSubeData
    {
        [LogAspect]
        public string UpdateWithNested(Sube entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<Sube>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;

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

        [LogAspect]
        public string DeleteWithNested(Sube entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    var yetkiler = context.PersoneliSubeYetkiler.Where(x => x.SubeId == entity.SubeId);

                    if(yetkiler != null && yetkiler.Any())
                    {
                        foreach (var yetki in yetkiler)
                        {
                            context.Entry(yetki).State = EntityState.Deleted;
                        }
                    }

                    context.Set<Sube>().Attach(entity);
                    context.Entry(entity).State = EntityState.Deleted;

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
