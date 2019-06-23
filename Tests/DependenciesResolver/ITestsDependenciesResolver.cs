using AppFmk.Interfaces;

namespace Tests.DependenciesResolver
{
    public interface ITestsDependenciesResolver
    {
        T Resolve<T>();
        T Resolve<T>(IBasePage basePage);
    }
}
