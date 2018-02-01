using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.Boilerplate.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RootMasterDetailPage : MasterDetailPage
    {
        public RootMasterDetailPage()
        {
            InitializeComponent();
        }
    }
}