using System.Threading.Tasks;

namespace Plugin.Boilerplate.ViewModels
{
    public class ExtendedSplashViewModel : ViewModelBase
    {
        public override async Task InitializeAsync(object navigationData)
        {
            IsBusy = true;
            await NavigationService.InitializeAsync();
            IsBusy = false;
        }
    }
}
