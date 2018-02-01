﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Plugin.Boilerplate.Models
{
    public abstract class BaseNotify : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool SetPropertyChanged<T>(ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            return PropertyChanged.SetProperty(this, ref currentValue, newValue, propertyName);
        }

        public void SetPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

namespace System.ComponentModel
{
    public static class BaseNotify
    {
        // Adding some new funk.tionality to System.ComponentModel
        public static bool SetProperty<T>(this PropertyChangedEventHandler handler, object sender, ref T currentValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
            {
                return false;
            }

            currentValue = newValue;

            if (sender is Plugin.Boilerplate.Models.IDirty dirty)
            {
                dirty.IsDirty = true;
            }

            if (handler == null)
            {
                return true;
            }

            handler.Invoke(sender, new PropertyChangedEventArgs(propertyName));

            return true;
        }
    }
}