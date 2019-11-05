using Ninject;
using Ninject.Modules;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Core.Utilities.MVC.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        IKernel kernel;

        public NinjectControllerFactory(INinjectModule module)
        {
            this.kernel = new StandardKernel(module);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)kernel.Get(controllerType);
        }
    }

}
