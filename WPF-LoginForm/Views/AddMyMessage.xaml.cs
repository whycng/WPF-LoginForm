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
using System.Windows.Shapes;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// AddMyMessage.xaml 的交互逻辑
    /// </summary>
    public partial class AddMyMessage : Window
    {
        public AddMyMessage()
        {
            InitializeComponent();
        }

        private void PnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove(); // 拖动窗口

            //WindowInteropHelper helper = new WindowInteropHelper(this);
            //SendMessage(helper.Handle, 161, 2, 0);
        }
        private void btnSave(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnCancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
