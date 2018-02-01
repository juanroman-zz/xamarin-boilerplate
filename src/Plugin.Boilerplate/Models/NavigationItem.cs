using System;
using Xamarin.Forms;

namespace Plugin.Boilerplate.Models
{
    public class NavigationItem : BaseNotify
    {
        private string _title;
        private bool _isEnabled;
        private FileImageSource _icon;
        private Type _viewModelType;

        public string Title
        {
            get => _title;
            set => SetPropertyChanged(ref _title, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetPropertyChanged(ref _isEnabled, value);
        }

        public FileImageSource Icon
        {
            get => _icon;
            set => SetPropertyChanged(ref _icon, value);
        }

        public Type ViewModelType
        {
            get => _viewModelType;
            set => SetPropertyChanged(ref _viewModelType, value);
        }
    }
}
