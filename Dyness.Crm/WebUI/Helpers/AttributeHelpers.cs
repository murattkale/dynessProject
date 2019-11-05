using Core.CrossCuttingConcerns.Security;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebUI.Helpers
{
    public static class AttributeHelpers
    {
        static PropertyInfo GetPropertyInfo<TSource, TProperty>(this TSource source, Expression<Func<TSource, TProperty>> propertyLambda)
        {
            try
            {
                var type = typeof(TSource);

                var member = propertyLambda.Body as MemberExpression;
                var propInfo = member.Member as PropertyInfo;
                return propInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }

        static string htmlTextDanger = "<span class=\"text-danger\">*</span>";

        public static bool GetIsRequired<TModel, TValue>(this TModel entity, Expression<Func<TModel, TValue>> propertyExpression)
        {
            try
            {
                var propertyInfo = entity.GetPropertyInfo(propertyExpression);

                var customAttribute = propertyInfo.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "RequiredAttribute");

                return customAttribute != null;
            }
            catch { return false; }
        }

        public static bool GetIsNullable<TModel, TValue>(this TModel entity, Expression<Func<TModel, TValue>> propertyExpression)
        {
            try
            {
                var propertyInfo = entity.GetPropertyInfo(propertyExpression);

                var propertyType = propertyInfo.PropertyType;

                return propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
            }
            catch { return false; }
        }

        public static string GetHtmlIfIsRequired<T, TProperty>(this T entity, Expression<Func<T, TProperty>> propertyExpression)
        {
            return GetIsRequired(entity, propertyExpression) ? htmlTextDanger : "";
        }

        public static string GetHtmlIfIsRequired(bool isRequired)
        {
            return isRequired ? htmlTextDanger : "";
        }

        public static string GetDisplayName<T, TProperty>(this T entity, Expression<Func<T, TProperty>> propertyExpression)
        {
            var name = (propertyExpression.Body as MemberExpression).Member.Name;

            try
            {
                var propertyInfo = entity.GetPropertyInfo(propertyExpression);

                var customAttribute = propertyInfo.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "DisplayAttribute");

                if (customAttribute != null)
                {
                    var resources = typeof(Core.Properties.FieldNameResources).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                    var item = resources.FirstOrDefault(x => x.Name == customAttribute.NamedArguments[0].TypedValue.Value.ToString());

                    return item.GetValue(null, null).ToString();
                }

                return name;
            }
            catch
            {
                return name;
            }
        }

        public static string GetColumnName<T, TProperty>(this T entity, Expression<Func<T, TProperty>> propertyExpression)
        {
            var propertyInfo = entity.GetPropertyInfo(propertyExpression);

            return propertyInfo.Name;
        }
    }

    public class CheckAuthorizedActionFilter : ActionFilterAttribute, IActionFilter
    {
        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            //var action = filterContext.ActionDescriptor.ActionName;

            //var dd = filterContext.ActionDescriptor.GetParameters();
            //var httpMethod = filterContext.HttpContext.Request.HttpMethod;
            //var returnType = ((ReflectedActionDescriptor)filterContext.ActionDescriptor).MethodInfo.ReturnType.Name;

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/kullanici/giris");
            }
            else if (Identity.Rol == RolModel.Ogrenci && controllerName.ToLower() != "ogrencibilgi")
            {
                filterContext.Result = new RedirectResult("/OgrenciBilgi/");
            }
            else if (Identity.Rol == RolModel.Personel && controllerName.ToLower() == "ogrencibilgi")
            {
                filterContext.Result = new RedirectResult("/");
            }
        }
    }
}