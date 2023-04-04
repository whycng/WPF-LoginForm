using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_LoginForm.ViewModels;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// CompassView.xaml 的交互逻辑
    /// </summary>
    public partial class CompassView : UserControl
    {
        public CompassView()
        {

            
            InitializeComponent();
            CompassViewModel viewModel = new CompassViewModel();
            viewModel.Query();

            this.DataContext = viewModel;

        }

        //private void UserControl_Loaded(object sender, RoutedEventArgs e)
        //{
        //    CompassViewModel viewModel = new CompassViewModel();
        //    viewModel.Query();

        //    // this.DataContext = viewModel;
        //}
    }
}
