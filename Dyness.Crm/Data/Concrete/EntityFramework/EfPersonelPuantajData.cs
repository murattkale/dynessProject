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
    public class EfPersonelPuantajData : EFEntityRepository<PersonelPuantaj, EFContext>, IEfPersonelPuantajData
    {
        [LogAspect]
        public string UpdateWithNestedLists(PersonelPuantaj entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    foreach (var personelPuantajGunluk in entity.PersonelPuantajGunlukler)
                    {
                        context.Set<PersonelPuantajGunluk>().Attach(personelPuantajGunluk);
                        context.Entry(personelPuantajGunluk).State = EntityState.Modified;
                    }

                    context.Set<PersonelPuantaj>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;

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
