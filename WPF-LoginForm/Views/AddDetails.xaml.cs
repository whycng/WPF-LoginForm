using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// AddDetails.xaml 的交互逻辑
    /// </summary>
    public partial class AddDetails : Window // DetailsModel
    {
        public AddDetails()
        {
            InitializeComponent();
 
        }
        //public AddDetails(DetailsModel details)
        //{
        //    InitializeComponent();
        //    this.DataContext = new
        //    {
        //        Model_details = details
        //    };
        //}

        // 样式已经被修改
        //private void btnSave(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = true;
        //}

        //private void btnCancel(object sender, RoutedEventArgs e)
        //{
        //    this.DialogResult = false;
        //}


        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void PnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // DragMove(); // 拖动窗口

            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }
        private void PnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            // Application.Current.Shutdown();
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                this.WindowState = WindowState.Normal;
            else this.WindowState = WindowState.Maximized;
        }
    }
}
