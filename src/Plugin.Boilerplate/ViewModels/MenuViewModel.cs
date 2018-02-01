using Plugin.Boilerplate.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Plugin.Boilerplate.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        public ObservableCollection<NavigationItem> MenuItems { get; set; }
    }
}
