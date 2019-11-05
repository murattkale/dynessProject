using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Data.Entity.Validation;

namespace Data.Concrete.EntityFramework
{
    public class EfSmsData : EFEntityRepository<Sms, EFContext>, IEfSmsData
    {
        [LogAspect]
        public string AddWithSmsHesapHareket(Sms entity)
        {
            string returnMessage;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<Sms>().Add(entity);

                    if (entity.SmsHesapHareket != null)
                        context.Set<SmsHesapHareket>().Add(entity.SmsHesapHareket);

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
