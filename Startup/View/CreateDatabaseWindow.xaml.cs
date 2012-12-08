using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Registry.ViewModel;
using Startup.ViewModel;

namespace Startup.View
{
    /// <summary>
    /// Interaction logic for CreateDatabaseWindow.xaml
    /// </summary>
    public partial class CreateDatabaseWindow : Window
    {
        public CreateDatabaseWindow()
        {
            InitializeComponent();

            CreateDatabaseWindowViewModel mwvm = new CreateDatabaseWindowViewModel();
            mwvm.RequestClose += delegate { this.Close(); };
            DataContext = mwvm;
        }
    }
}
