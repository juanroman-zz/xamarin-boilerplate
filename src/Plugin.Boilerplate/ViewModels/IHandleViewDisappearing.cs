using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plugin.Boilerplate.ViewModels
{
    public interface IHandleViewDisappearing
    {
        Task OnViewDisappearingAsync(VisualElement view);
    }
}
