using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Autodesk.Revit.UI;
using Masarr_Revit_Plugin.ViewModels;
using Masarr_Revit_Plugin.Views;

namespace Masarr_Revit_Plugin.Views

{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(ExternalCommandData commandData)
        {
            InitializeComponent();
            DataContext = new ViewModels.MainViewModel(commandData);
        }
    }
}
