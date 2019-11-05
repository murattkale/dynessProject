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
    public class EfOgrenciData : EFEntityRepository<Ogrenci, EFContext>, IEfOgrenciData
    {
        [LogAspect]
        public string UpdateWithNested(Ogrenci entity)
        {
            var returnMessage = string.Empty;

            try
            {
                using (var context = new EFContext())
                {
                    context.Set<Ogrenci>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;

                    context.Set<OgrenciYakiniIletisim>().Attach(entity.AnneOgrenciYakiniIletisim);
                    context.Entry(entity.AnneOgrenciYakiniIletisim).State = EntityState.Modified;

                    context.Set<OgrenciYakiniIletisim>().Attach(entity.BabaOgrenciYakiniIletisim);
                    context.Entry(entity.BabaOgrenciYakiniIletisim).State = EntityState.Modified;

                    context.Set<OgrenciYakiniIletisim>().Attach(entity.YakiniOgrenciYakiniIletisim);
                    context.Entry(entity.YakiniOgrenciYakiniIletisim).State = EntityState.Modified;

                    context.Set<Hesap>().Attach(entity.Hesap);
                    context.Entry(entity.Hesap).State = EntityState.Modified;

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
