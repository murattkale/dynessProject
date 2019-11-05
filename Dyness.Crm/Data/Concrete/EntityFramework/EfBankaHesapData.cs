using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Data.Concrete.EntityFramework
{
    public class EfBankaHesapData : EFEntityRepository<BankaHesap, EFContext>, IEfBankaHesapData
    {
        [LogAspect]
        public string UpdateWithNested(BankaHesap entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<BankaHesap>().Attach(entity);
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
    }
}
