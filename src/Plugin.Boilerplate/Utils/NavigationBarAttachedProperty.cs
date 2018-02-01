using Plugin.Boilerplate.Pages;
using Xamarin.Forms;

namespace Plugin.Boilerplate.Utils
{
    public static class NavigationBarAttachedProperty
    {
        public static readonly BindableProperty TextColorProperty = BindableProperty.CreateAttached(
            "TextColor",
            typeof(Color),
            typeof(NavigationBarAttachedProperty),
            Color.Default);

        public static Color GetTextColor(BindableObject bindableObject)
        {
            return (Color)bindableObject.GetValue(TextColorProperty);
        }

        public static void SetTextColor(BindableObject bindableObject, Color value)
        {
            bindableObject.SetValue(TextColorProperty, value);

            if (bindableObject is Page page && null != page.Parent && page.Parent is CustomNavigationPage customNavigationPage)
            {
                customNavigationPage.ApplyNavigationTextColor(page);
            }
        }
    }
}
