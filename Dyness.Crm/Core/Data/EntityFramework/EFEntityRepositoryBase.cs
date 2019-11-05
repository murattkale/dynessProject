using Core.Aspects.Postsharp.LogAspects;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Data.EntityFramework
{
    public class EFEntityRepository<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, new()
        where TContext : DbContext, new()
    {
        public static string ReturnDbEntityValidationExceptionMessage(DbEntityValidationException dbEntityValidationException)
        {
            var errorMessage = "";

            foreach (var eve in dbEntityValidationException.EntityValidationErrors)
            {
                errorMessage += $"Entity : \"{eve.Entry.Entity.GetType().Name}\", error : \"{eve.Entry.State}\", validate errors : ";
                errorMessage += eve.ValidationErrors.Aggregate(errorMessage, (current, ve) => current + $" property : \"{ve.PropertyName}\", error : \"{ve.ErrorMessage}\";");
            }

            return errorMessage;
        }

        public static string ReturnExceptionMessage(TEntity entity, Exception exception)
        {
            return $"{(entity != null ? $"Entity : \"{entity.GetType().Name}\", " : string.Empty)}error-message : \"{exception.Message}, error-inner-exception : \"{exception.InnerException}\"";
        }

        [LogAspect]
        public string Add(TEntity entity)
        {
            try
            {
                using (var context = new TContext())
                {
                    context.Set<TEntity>().Add(entity);
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                return ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                return ReturnExceptionMessage(entity, exception);
            }
        }

        [LogAspect]
        public string AddOrUpdate(TEntity entity)
        {
            try
            {
                using (var context = new TContext())
                {
                    context.Set<TEntity>().AddOrUpdate(entity);
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                return ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                return ReturnExceptionMessage(entity, exception);
            }
        }

        [LogAspect]
        public string Update(TEntity entity)
        {
            try
            {
                using (var context = new TContext())
                {
                    context.Set<TEntity>().Attach(entity);
                    context.Entry(entity).State = EntityState.Modified;
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                return ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                return ReturnExceptionMessage(entity, exception);
            }
        }

        [LogAspect]
        public string Delete(TEntity entity)
        {
            try
            {
                using (var context = new TContext())
                {
                    context.Set<TEntity>().Remove(entity);
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                return ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                return ReturnExceptionMessage(entity, exception);
            }
        }

        [LogAspect]
        public string DeleteById(int id)
        {
            TEntity entity = null;

            try
            {
                using (var context = new TContext())
                {
                    entity = GetById(id);
                    context.Entry(entity).State = EntityState.Deleted;
                    context.SaveChanges();
                    return string.Empty;
                }
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                return ReturnDbEntityValidationExceptionMessage(dbEntityValidationException);
            }
            catch (Exception exception)
            {
                return ReturnExceptionMessage(entity, exception);
            }
        }

        public int GetCount()
        {
            using (var context = new TContext())
            {
                var query = context.Set<TEntity>();

                return query.Count();
            }
        }

        public int GetByWhereCaseCount(Expression<Func<TEntity, bool>> where)
        {
            using (var context = new TContext())
            {
                var query = context.Set<TEntity>();

                return query.Where(where).Count();
            }
        }

        public TEntity GetById(int id)
        {
            using (var context = new TContext())
            {
                var dbSet = context.Set<TEntity>();
                var entity = dbSet.Find(id);

                return entity;
            }
        }

        public TEntity GetByWhereCaseIncludeMultipleFirstOrDefault(
            Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] includes)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (includes != null)
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                var model = where == null
                    ? query.FirstOrDefault()
                    : query.FirstOrDefault(where);

                return model;
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            using (var context = new TContext())
            {
                var query = context.Set<TEntity>();

                return query.ToList();
            }
        }

        public IEnumerable<TEntity> GetAllIncludeMultiple(params Expression<Func<TEntity, object>>[] includes)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (includes != null)
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                return query.ToList();
            }
        }

        public IEnumerable<TEntity> GetByWhereCase(Expression<Func<TEntity, bool>> where)
        {
            using (var context = new TContext())
                return context.Set<TEntity>().Where(where).ToList();
        }

        public IEnumerable<TEntity> GetByOrderByTake<TKey>(
            Expression<Func<TEntity, TKey>> orderBy,
            int take,
            bool desceding)
        {
            using (var context = new TContext())
            {
                switch (desceding)
                {
                    case true:
                        return context.Set<TEntity>().OrderByDescending(orderBy).Take(take).ToList();
                    default:
                        return context.Set<TEntity>().OrderBy(orderBy).Take(take).ToList();
                }
            }
        }

        public IEnumerable<TEntity> GetByWhereCaseIncludeMultiple(
            Expression<Func<TEntity, bool>> where,
            params Expression<Func<TEntity, object>>[] includes)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (includes != null)
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                var model = where == null
                    ? query.ToList()
                    : query.Where(where).ToList();

                return model;
            }
        }

        public IEnumerable<TEntity> GetByWhereCaseByOrderByTakeIncludeMultiple<TKey>(
            Expression<Func<TEntity, bool>> where,
            Expression<Func<TEntity, TKey>> orderBy,
            int take,
            bool desceding,
            params Expression<Func<TEntity, object>>[] includes)
        {
            using (var context = new TContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

                if (includes != null)
                    query = includes.Aggregate(query, (current, include) => current.Include(include));

                switch (desceding)
                {
                    case true:
                        return query.Where(where).OrderByDescending(orderBy).Take(take).ToList();
                    default:
                        return query.Where(where).OrderBy(orderBy).Take(take).ToList();
                }
            }
        }
    }
}
