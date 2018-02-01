﻿using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Plugin.Boilerplate.MarkupExtensions
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == Text)
            {
                return null;
            }

            var assembly = typeof(TranslateExtension).GetTypeInfo().Assembly;
            var assemblyName = assembly.GetName();
            var resourceManager = new ResourceManager($"{assemblyName.Name}.Resources", assembly);

            return resourceManager.GetString(Text, CultureInfo.CurrentCulture);
        }
    }
}
