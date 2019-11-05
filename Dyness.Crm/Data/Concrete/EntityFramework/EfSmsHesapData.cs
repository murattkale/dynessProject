using Core.Aspects.Postsharp.LogAspects;
using Core.Data.EntityFramework;
using Data.Abstract.EntityFramework;
using Data.Concrete.EntityFramework.Context;
using Entities.Concrete;
using System;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;

namespace Data.Concrete.EntityFramework
{
    public class EfSmsHesapData : EFEntityRepository<SmsHesap, EFContext>, IEfSmsHesapData
    {
        [LogAspect]
        public string UpdateWithNestedLists(SmsHesap entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    if (entity.SmsHesapDosyalar != null && entity.SmsHesapDosyalar.Any())
                    {
                        foreach (var smsHesapDosya in entity.SmsHesapDosyalar)
                        {
                            smsHesapDosya.SmsHesap = null;

                            if(smsHesapDosya.SmsHesapDosyaId == 0 && !string.IsNullOrEmpty(smsHesapDosya.DosyaAd))
                                context.Set<SmsHesapDosya>().AddOrUpdate(smsHesapDosya);
                        }

                        entity.SmsHesapDosyalar = null;

                        context.Set<SmsHesap>().AddOrUpdate(entity);
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
