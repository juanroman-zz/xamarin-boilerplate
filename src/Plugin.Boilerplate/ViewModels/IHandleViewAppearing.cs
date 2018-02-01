using System.Threading.Tasks;
using Xamarin.Forms;

namespace Plugin.Boilerplate.ViewModels
{
    public interface IHandleViewAppearing
    {
        Task OnViewAppearingAsync(VisualElement view);
    }
}
