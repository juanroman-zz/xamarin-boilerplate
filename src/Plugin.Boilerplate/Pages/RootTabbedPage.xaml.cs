using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.Boilerplate.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootTabbedPage : TabbedPage
    {
        public RootTabbedPage()
        {
            InitializeComponent();
        }
    }
}