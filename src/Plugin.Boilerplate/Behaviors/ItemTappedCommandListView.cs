using System.Windows.Input;
using Xamarin.Forms;

namespace Plugin.Boilerplate.Behaviors
{
    public static class ItemTappedCommandListView
    {
        public static readonly BindableProperty ItemTappedCommandProperty = BindableProperty.Create(
            "ItemTappedCommand",
            typeof(ICommand),
            typeof(ItemTappedCommandListView),
            default(ICommand),
            BindingMode.OneWay,
            null,
            PropertyChanged);

        private static void PropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is ListView listView)
            {
                listView.ItemTapped -= ListViewOnItemTapped;
                listView.ItemTapped += ListViewOnItemTapped;
            }
        }

        private static void ListViewOnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView listView && listView.IsEnabled && !listView.IsRefreshing)
            {
                listView.SelectedItem = null;

                var command = GetItemTappedCommand(listView);
                if (command?.CanExecute(e.Item) == true)
                {
                    command.Execute(e.Item);
                }
            }
        }

        private static ICommand GetItemTappedCommand(BindableObject bindableObject)
        {
            return (ICommand)bindableObject.GetValue(ItemTappedCommandProperty);
        }

        private static void SetItemTappedCommand(BindableObject bindableObject, object value)
        {
            bindableObject.SetValue(ItemTappedCommandProperty, value);
        }
    }
}
