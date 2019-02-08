using System;

namespace Fulton
{
    public interface IContainer
    {
        void Register(Type dependency, Type resolve);
        void Register<TDependency>(Func<IContainer, TDependency> factory);
        void Register<TDependency, TResolve>() where TResolve : TDependency;
        object Resolve(string key, Type[] genericParameters, ResolveContext context);
        T Resolve<T>();
    }
}