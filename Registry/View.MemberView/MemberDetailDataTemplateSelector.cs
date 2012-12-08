using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Markup;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Registry.View.MemberView
{
    public class MemberDetailDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageTemplate { get; set; }
        public DataTemplate StringTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return StringTemplate;

            return ImageTemplate;
        }
    }

}
