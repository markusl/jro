using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Globalization;

namespace Registry.View.MemberView
{
    /// <summary>
    /// ValueConverter to check whether a string value is null or empty.
    /// </summary>
    public class NullOrEmptyConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if string value is null or empty.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return String.IsNullOrEmpty(value as string);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }
    }

    /// <summary>
    /// ValueConverter to convert boolean to Visibility.
    /// </summary>
    public class VisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if string value is null or empty.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return val ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }
    }

    /// <summary>
    /// ValueConverter to convert negative boolean to Visibility.
    /// </summary>
    public class NegativeVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Returns true if string value is null or empty.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            return val ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("The method or operation is not implemented.");
        }
    }
}
