using Plugin.Boilerplate.Models;
using Plugin.Boilerplate.Services.Dialog;
using Plugin.Boilerplate.Services.Navigation;
using System.Threading.Tasks;

namespace Plugin.Boilerplate.ViewModels
{
    public abstract class ViewModelBase : BaseNotify
    {
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        private bool _isBusy;
        private string _title;

        protected ViewModelBase()
        {
            _dialogService = LocatorBase.Instance.Resolve<IDialogService>();
            _navigationService = LocatorBase.Instance.Resolve<INavigationService>();
        }

        public IDialogService DialogService => _dialogService;
        public INavigationService NavigationService => _navigationService;

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
