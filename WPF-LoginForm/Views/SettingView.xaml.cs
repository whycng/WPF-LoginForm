using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// SettingView.xaml 的交互逻辑
    /// </summary>
    public partial class SettingView : UserControl
    {
        public SettingView()
        {
            InitializeComponent();
        }

        private void OpenWebPage(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/whycng/WPF-LoginForm");
        }

        public double StringToDouble(string value)
        {
            if (double.TryParse(value, out double result))
            {
                return result;
            }
            return 0.0;
        }

    }
}
