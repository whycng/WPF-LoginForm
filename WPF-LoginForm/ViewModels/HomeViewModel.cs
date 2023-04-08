using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
 

namespace WPF_LoginForm.ViewModels
{
 
    public class HomeViewModel : ViewModelBase
    {
        //Fields
        private UserAccountModel _currentUserAccount; 
        private ViewModelBase _currentChildView;
        private string _caption; // caption 右侧视图左上角-名称
        private IconChar _icon; // icon 右侧视图左上角-图标
   


        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
            }
        }

        public ViewModelBase CurrentChildView
        {
            get
            {
                return _currentChildView;
            } // => _currentChildView;
            set
            {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }// => _currentChildView = value;
        }
    
        public string Caption
        {
            get
            {
                return _caption;
            } // => _caption;
            set
            {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            } // => _caption = value;
        }
        public IconChar Icon
        {
            get
            {
                return _icon;
            } // => _icon;
            set
            {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            } // => _icon = value;
        }

        // --> Commands
        public ICommand ShowHomeViewCommand { get; }
        public ICommand ShowFoodViewCommand { get; }
        public ICommand ShowFruitViewCommand { get; }
        public ICommand ShowBookViewCommand { get; }
        public ICommand ShowBhViewCommand { get; }
        public ICommand ShowMedicineViewCommand { get; }


        public HomeViewModel() // 构造函数，初始化使用 view
        {
             
            // Initialize commands
            // 推荐 recommend
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            // food 
            ShowFoodViewCommand = new ViewModelCommand(ExecuteShowFoodViewCommand);
            ShowFruitViewCommand = new ViewModelCommand(ExecuteShowFruitViewCommand);
            ShowBookViewCommand = new ViewModelCommand(ExecuteShowBookViewCommand);
            ShowBhViewCommand = new ViewModelCommand(ExecuteShowBhViewCommand);
            ShowMedicineViewCommand = new ViewModelCommand(ExecuteShowMedicineViewCommand);



            // Default View
            ExecuteShowHomeViewCommand(null);

             
        }

        private void ExecuteShowBookViewCommand(object obj)
        { 
            CurrentChildView = new HomeViewModel_book();
            Caption = "Book";
            Icon = IconChar.Compass;
        }

        private void ExecuteShowBhViewCommand(object obj)
        { 
            CurrentChildView = new HomeViewModel_bh();
            Caption = "bh 百货";
            Icon = IconChar.Gear;
        }

        private void ExecuteShowFruitViewCommand(object obj)
        { 
            CurrentChildView = new HomeViewModel_frui();
            Caption = "Message-MainViewModel";
            Icon = IconChar.Message;
        }

        private void ExecuteShowFoodViewCommand(object obj)
        { 
            CurrentChildView = new HomeViewModel_food();
            Caption = "Customer";
            Icon = IconChar.UserGroup;
            // throw new NotImplementedException();
        }

        private void ExecuteShowHomeViewCommand(object obj)
        { 
            CurrentChildView = new HomeViewModel_reco(); // 推荐
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        private void ExecuteShowMedicineViewCommand(object obj)
        {
            CurrentChildView = new HomeViewModel_medi(); // 推荐
        }
 
    }
}
