using Plugin.Boilerplate.Pages;
using Plugin.Boilerplate.Services.Authentication;
using Plugin.Boilerplate.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plugin.Boilerplate.Services.Navigation
{
    public class NavigationServiceBase<THomeViewModel, TLoginViewModel> : INavigationService
        where THomeViewModel : ViewModelBase
        where TLoginViewModel : ViewModelBase
    {
        private readonly IAuthenticationService _authenticationService;

        protected NavigationServiceBase(IAuthenticationService authenticationService)
        {
            Mappings = new Dictionary<Type, Type>();
            _authenticationService = authenticationService;
        }

        protected Dictionary<Type, Type> Mappings { get; }

        public async Task InitializeAsync()
        {
            if (await _authenticationService.UserIsAuthenticatedAndValidAsync())
            {
                await NavigateToAsync<MasterViewModel>();
            }
            else
            {
                await NavigateToAsync<TLoginViewModel>();
            }
        }

        public Task NavigateBackAsync()
        {
            if (Application.Current.MainPage is RootMasterDetailPage rootMasterDetailPage && null != rootMasterDetailPage.Detail)
            {
                return rootMasterDetailPage.Detail.Navigation.PopAsync(true);
            }
            else if (Application.Current.MainPage is RootTabbedPage rootTabbedPage && null != rootTabbedPage.CurrentPage)
            {
                return rootTabbedPage.CurrentPage.Navigation.PopAsync(true);
            }

            return Task.FromResult(false);
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToAsync(Type viewModelType)
        {
            return InternalNavigateToAsync(viewModelType, null);
        }

        public Task NavigateToAsync(Type viewModelType, object parameter)
        {
            return InternalNavigateToAsync(viewModelType, parameter);
        }

        public Task RemoveLastFromBackStackAsync() => throw new NotImplementedException();

        protected virtual void CreatePageViewModelMappings()
        {
            Mappings.Add(typeof(ExtendedSplashViewModel), typeof(ExtendedSplashPage));
            Mappings.Add(typeof(MenuViewModel), typeof(MenuPage));
        }

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            if (typeof(ExtendedSplashViewModel) == viewModelType || typeof(TLoginViewModel) == viewModelType)
            {
                Page page = CreateAndBindPage(viewModelType);
                if (typeof(ExtendedSplashViewModel) == viewModelType)
                {
                    Application.Current.MainPage = page;
                }
                else
                {
                    Application.Current.MainPage = new CustomNavigationPage(page);
                }

                await ((ViewModelBase)page.BindingContext).InitializeAsync(parameter);
            }
            else if (Device.iOS == Device.RuntimePlatform && TargetIdiom.Phone == Device.Idiom)
            {
                await InternalNavigateToInTabsAsync(viewModelType, parameter);
            }
            else
            {
                await InternalNavigateToInMasterDetailAsync(viewModelType, parameter);
            }
        }

        private async Task InternalNavigateToInTabsAsync(Type viewModelType, object parameter)
        {
            if (typeof(MasterViewModel) == viewModelType)
            {
                var masterViewModel = LocatorBase.Instance.Resolve<MasterViewModel>();

                var tabbedPage = new RootTabbedPage
                {
                    BindingContext = masterViewModel,
                    Title = masterViewModel.Title
                };

                var menuViewModel = LocatorBase.Instance.Resolve<MenuViewModel>();
                foreach (var menuItem in menuViewModel.MenuItems)
                {
                    var childPageType = GetPageTypeForViewModel(menuItem.ViewModelType);
                    var childPage = Activator.CreateInstance(childPageType) as Page;
                    var navigationPage = new CustomNavigationPage(childPage)
                    {
                        Title = menuItem.Title,
                        Icon = menuItem.Icon
                    };

                    tabbedPage.Children.Add(navigationPage);
                }

                Application.Current.MainPage = tabbedPage;
            }
            else if (Application.Current.MainPage is RootTabbedPage tabbedPage)
            {
                Page page = CreateAndBindPage(viewModelType);

                await tabbedPage.CurrentPage.Navigation.PushAsync(page);
                await ((ViewModelBase)page.BindingContext).InitializeAsync(parameter);
            }
        }

        private async Task InternalNavigateToInMasterDetailAsync(Type viewModelType, object parameter)
        {
            if (typeof(MasterViewModel) == viewModelType)
            {
                var masterViewModel = LocatorBase.Instance.Resolve<MasterViewModel>();
                var menuPage = CreateAndBindPage(typeof(MenuViewModel));
                var detailPage = CreateAndBindPage(typeof(THomeViewModel));

                Application.Current.MainPage = new RootMasterDetailPage
                {
                    BindingContext = masterViewModel,
                    Master = menuPage,
                    Detail = new CustomNavigationPage(detailPage)
                    {
                        Title = detailPage.Title
                    }
                };

                await ((ViewModelBase)detailPage.BindingContext).InitializeAsync(parameter);
            }
            else if (Application.Current.MainPage is RootMasterDetailPage masterDetailPage)
            {
                Page page = CreateAndBindPage(viewModelType);
                var menuViewModel = LocatorBase.Instance.Resolve<MenuViewModel>();
                var menuItemTypes = menuViewModel.MenuItems.Select(mi => mi.ViewModelType);

                if (menuItemTypes.Contains(viewModelType))
                {
                    masterDetailPage.Detail = new CustomNavigationPage(page)
                    {
                        Title = page.Title
                    };
                }
                else if (masterDetailPage.Detail is CustomNavigationPage navigationPage)
                {
                    await navigationPage.PushAsync(page, true);
                }
                else
                {
                    masterDetailPage.Detail = new CustomNavigationPage(page)
                    {
                        Title = page.Title
                    };
                }

                masterDetailPage.IsPresented = false;
                await ((ViewModelBase)page.BindingContext).InitializeAsync(parameter);
            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            if (!Mappings.ContainsKey(viewModelType))
            {
                throw new KeyNotFoundException($"No map for {viewModelType} was found on navigation mappings.");
            }

            return Mappings[viewModelType];
        }

        private Page CreateAndBindPage(Type viewModelType)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);
            if (null == pageType)
            {
                throw new Exception($"Mapping type for {viewModelType} is not a Page.");
            }

            var viewModel = LocatorBase.Instance.Resolve(viewModelType) as ViewModelBase;
            Page page = Activator.CreateInstance(pageType) as Page;
            page.BindingContext = viewModel;
            page.Title = viewModel.Title;

            return page;
        }
    }
}
