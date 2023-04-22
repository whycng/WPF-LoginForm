using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using WPF_LoginForm.Models;
using System.Windows.Controls;

namespace WPF_LoginForm.ViewModels
{   // abstract:只能通过继承使用？
    public abstract class ViewModelBase : INotifyPropertyChanged 
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DetailsModel _detailsModelTmp;
        public DetailsModel DetailsModelTmp
        {
            get {
                return _detailsModelTmp; 
            }
            set { _detailsModelTmp = value; 
               // OnPropertyChanged(nameof(DetailsModelTmp)); 
            }
        }
        public void OnPropertyChanged(string propertyName)
        {
            // 当属性改变时引发事件
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
 
        public ViewModelBase()
        {
             
        }
    }
}
