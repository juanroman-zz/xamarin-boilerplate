using Plugin.Boilerplate.Models;
using Plugin.Boilerplate.Services.Dialog;
using Plugin.Boilerplate.Services.Navigation;
using System.Threading.Tasks;

namespace Plugin.Boilerplate.ViewModels
{
    public abstract class ViewModelBase : BaseNotify
    {
        private bool _isBusy;
        private string _title;

        protected ViewModelBase()
        {
            DialogService = LocatorBase.Instance.Resolve<IDialogService>();
            NavigationService = LocatorBase.Instance.Resolve<INavigationService>();
        }

        public IDialogService DialogService { get; }
        public INavigationService NavigationService { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetPropertyChanged(ref _isBusy, value);
        }

        public string Title
        {
            get => _title;
            set => SetPropertyChanged(ref _title, value);
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }
    }
}
