using System;

namespace Plugin.Boilerplate.ViewModels
{
    public interface ILocator
    {
        void Build();

        void Register<TInterface, TImplementation>() where TImplementation : TInterface;

        void Register<T>() where T : class;

        T Resolve<T>() where T : class;

        object Resolve(Type type);
    }
}
