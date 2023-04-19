using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;

namespace WPF_LoginForm.ViewModels //
{
    public class MainViewModel : ViewModelBase
    {
        //Fields
        private UserAccountModel _currentUserAccount;
        private IUserRepository userRepository;
        private ViewModelBase _currentChildView;
        private string _caption; // caption 右侧视图左上角-名称
        private IconChar _icon; // icon 右侧视图左上角-图标
        private string _testN;
        private Uri _userPhoto;
        public Uri UserPhoto
        {
            get => _userPhoto;
            set
            {
                _userPhoto = value;
                OnPropertyChanged(nameof(UserPhoto));
            }//=> _testusername = value;
        }

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

        public ViewModelBase CurrentChildView {
            get {
                return _currentChildView;
            } // => _currentChildView;
            set {
                _currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }// => _currentChildView = value;
        }
        public string TestN
        {
            get
            {
                return _testN;
            }//=> _testN;
            set
            {
                _testN = value;
                OnPropertyChanged(nameof(TestN));
            }//=> _testN = value;
        }
        public string Caption {
            get {
                return _caption;
            } // => _caption;
            set {
                _caption = value;
                OnPropertyChanged(nameof(Caption));
            } // => _caption = value;
        }
        public IconChar Icon {
            get {
                return _icon;
            } // => _icon;
            set {
                _icon = value;
                OnPropertyChanged(nameof(Icon));
            } // => _icon = value;
        }

        // --> Commands
        public ICommand ShowHomeViewCommand { get;  }
        public ICommand ShowCustomerViewCommand { get; }
        public ICommand ShowMessageViewCommand { get; }
        public ICommand ShowSettingViewCommand { get; }
        public ICommand ShowCompassViewCommand { get; }//
        public ICommand ShowMerchantViewCommand { get; }// 
        public ICommand ShowAppealViewCommand { get; }//
        public ICommand ShowAdminViewCommand { get; }//ShowAdminViewCommand


        public MainViewModel() // 构造函数，初始化使用 view
        {


            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();

            // Initialize commands
            ShowHomeViewCommand = new ViewModelCommand(ExecuteShowHomeViewCommand);
            ShowCustomerViewCommand = new ViewModelCommand(ExecuteShowCustomerViewCommand);
            ShowMessageViewCommand = new ViewModelCommand(ExecuteShowMessageViewCommand);
            ShowSettingViewCommand = new ViewModelCommand(ExecuteShowSettingViewCommand);
            ShowCompassViewCommand = new ViewModelCommand(ExecuteShowCompassViewCommand);
            ShowMerchantViewCommand = new ViewModelCommand(ExecuteShowMerchantViewCommand);
            ShowAppealViewCommand = new ViewModelCommand(ExecuteShowAppealViewCommand);
            ShowAdminViewCommand = new ViewModelCommand(ExecuteShowAdminViewCommand);

            // Default View
            ExecuteShowHomeViewCommand(null);


            LoadCurrentUserData();
        }//

        private void ExecuteShowAdminViewCommand(object obj)
        {

            CurrentChildView = new AdminViewModel();
            Caption = "管理员";
            Icon = IconChar.Dog;
        }
        private void ExecuteShowAppealViewCommand(object obj)
        {

            CurrentChildView = new AppealViewModel();
            Caption = "申诉";
            Icon = IconChar.Cat;
        }
        private void ExecuteShowMerchantViewCommand(object obj)
        {
           
            CurrentChildView = new MerchantViewModel();
            Caption = "商家";
            Icon = IconChar.Fish;
        }
        private void ExecuteShowCompassViewCommand(object obj)
        {
             
            CurrentChildView = new CompassViewModel();
            Caption = "订单";
            Icon = IconChar.Compass;
        }

        private void ExecuteShowSettingViewCommand(object obj)
        {
            
            CurrentChildView = new SettingViewModel();
            Caption = "Setting";
            Icon = IconChar.Gear;
        }

        private void ExecuteShowMessageViewCommand(object obj)
        {
          
            CurrentChildView = new ForMessageViewModel();
            Caption = "Message";  
            Icon = IconChar.Message;
        }

        private void ExecuteShowCustomerViewCommand(object obj)
        {
             
            CurrentChildView = new CustomerViewModel();
            Caption = "Customer";
            Icon = IconChar.UserGroup;  
            // throw new NotImplementedException();
        }

        private void ExecuteShowHomeViewCommand(object obj)
        {
            // TestN = "testname in Home";
            CurrentChildView = new HomeViewModel();
            Caption = "Dashboard";
            Icon = IconChar.Home;
        }

        private void LoadCurrentUserData()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null)
            {
                UserPhoto = user.UserPhoto;
                CurrentUserAccount.Username = user.Username;
                CurrentUserAccount.DisplayName = $"{user.Name} @{user.Username}";
                CurrentUserAccount.ProfilePicture = null;               
            }
            else
            {
                CurrentUserAccount.DisplayName="Invalid user, not logged in";
                //Hide child views.
            }
        }
    }
}
