using Autofac;
using Plugin.Boilerplate.Services.Dialog;
using Plugin.Boilerplate.Services.Request;
using System;

namespace Plugin.Boilerplate.ViewModels
{
    public class LocatorBase : ILocator
    {
        private readonly ContainerBuilder _containerBuilder;
        private IContainer _container;

        protected LocatorBase()
        {
            _containerBuilder = new ContainerBuilder();

            Register<IDialogService, DialogService>();
            Register<IRequestService, RequestService>();

            Register<MasterViewModel>();
            Register<MenuViewModel>();
            Register<ExtendedSplashViewModel>();
        }

        public static ILocator Instance { get; protected set; }

        public virtual void Build()
        {
            _container = _containerBuilder.Build();
        }

        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _containerBuilder.RegisterType<TImplementation>().As<TInterface>();
        }

        public void Register<T>() where T : class
        {
            _containerBuilder.RegisterType<T>();
        }

        public T Resolve<T>() where T : class
        {
            if (null == _container)
            {
                throw new InvalidOperationException("You need to Build the Locator.");
            }

            return _container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            if (null == _container)
            {
                throw new InvalidOperationException("You need to Build the Locator.");
            }

            return _container.Resolve(type);
        }
    }
}
