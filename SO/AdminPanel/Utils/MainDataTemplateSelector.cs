﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace AdminPanel.Utils
{
    public sealed class MainDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null || Application.Current == null)
                return null;

            string name = item.GetType().Name.Replace("ViewModel", "");
            var template = (DataTemplate)Application.Current.TryFindResource(name);

            if (template == null)
                throw new ArgumentException(name + " is not found");

            return template;
        }
    }
}
