using Core.Services;

namespace Services.DependencyResolvers
{
    public interface IServiceFactory
    {
        T CreateService<T>() where T : IServiceBase;

        void Release(IServiceBase service);
    }
}
