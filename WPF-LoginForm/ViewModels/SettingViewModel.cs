using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using WPF_LoginForm.Models;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        public SettingViewModel() { 
            
        }
        //属性

        //命令
        private RelayCommand _testStoreCommand;
        public RelayCommand TestStoreCommand
        {
            get
            {
                if (_testStoreCommand == null)
                    _testStoreCommand = new RelayCommand(() => ExecuteTestStoreCommand());
                return _testStoreCommand;
            }
        }
        //函数
        void ExecuteTestStoreCommand()
        { 
            StoreView view = new StoreView();
            var r = view.ShowDialog();
        }

    }
}