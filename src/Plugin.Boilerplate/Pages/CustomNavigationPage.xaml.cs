using Plugin.Boilerplate.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.Boilerplate.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomNavigationPage : NavigationPage
    {
        public CustomNavigationPage() : base()
        {
            InitializeComponent();
        }

        public CustomNavigationPage(Page root) : base(root)
        {
            InitializeComponent();
        }

        public void ApplyNavigationTextColor(Page page)
        {
            var color = NavigationBarAttachedProperty.GetTextColor(page);
            BarTextColor = color == Color.Default ? Color.White : color;
        }
    }
}