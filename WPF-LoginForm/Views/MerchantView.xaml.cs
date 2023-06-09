﻿using System;
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

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// MerchantView.xaml 的交互逻辑
    /// </summary>
    public partial class MerchantView : UserControl
    {
        public MerchantView()
        {
            InitializeComponent();
        }

        private void HandlePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text[0])) // 如果不是数字就不让输入
            {
                e.Handled = true;
            }
        }
    }
}
