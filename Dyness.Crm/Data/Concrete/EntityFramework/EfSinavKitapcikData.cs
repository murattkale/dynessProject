using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;

namespace Data.Concrete.EntityFramework
{
    public class EfSinavKitapcikData : EFEntityRepository<SinavKitapcik, EFContext>, IEfSinavKitapcikData
    {
        [LogAspect]
        public string UpdateWithNested(SinavKitapcik entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    var sinavKitapciklar = entity.SinavKitapcikDersBilgiler;
                    entity.SinavKitapcikDersBilgiler = null;

                    context.Set<SinavKitapcik>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;

                    if(sinavKitapciklar != null && sinavKitapciklar.Any())
                    {
                        foreach (var sinavKitapcikDersBilgi in sinavKitapciklar)
                        {
                            context.Set<SinavKitapcikDersBilgi>().AddOrUpdate(sinavKitapcikDersBilgi);
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
