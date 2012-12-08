using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Globalization;
using System.Threading.Tasks;
using Startup.View;
using Registry.ViewModel;

namespace Startup
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            // Ensure the current culture passed into bindings is the OS culture.
            // By default, WPF uses en-US as the culture, regardless of the system settings.
            FrameworkElement.LanguageProperty.OverrideMetadata(
              typeof(FrameworkElement),
              new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            DebugLogViewModel.Instance.StartListening();
        }

        /// <summary>
        /// Provides command line arguments passed to the application.
        /// </summary>
        public static string[] Args { get; private set; }

        public void Application_Startup(object sender, StartupEventArgs sea)
        {
            Args = sea.Args;
        }
    }
}
