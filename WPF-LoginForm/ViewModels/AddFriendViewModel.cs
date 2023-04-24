using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using GalaSoft.MvvmLight.Command;
using WPF_LoginForm.Repositories;


namespace WPF_LoginForm.ViewModels
{
    public class AddFriendViewModel:ViewModelBase
    {
        public AddFriendViewModel() {

            SearchFriCommand = new RelayCommand(() => ExecuteSearchFriCommand());
            // 信息初始化
            Reason = string.Empty;
            IsSaveEnabled = true;// 无用
            Model_user = null;
            userRepository = new UserRepository();
        }
        // 命令
        
        public RelayCommand SearchFriCommand { get; set; }
        // 属性
        private IUserRepository userRepository;
        private UserModel _model_user;// 添加的用户的信息
        public UserModel Model_user
        {
            get { return _model_user; }
            set { _model_user = value;
                OnPropertyChanged("Model_user");
            }
        }
        private string _reason;
        public string Reason
        {
            get { return _reason; }
            set { _reason = value;
                OnPropertyChanged("Reason");
            }
        }

        private bool _isSaveEnabled;

        public bool IsSaveEnabled
        {
            get { return _isSaveEnabled; }
            set
            {
                if (_isSaveEnabled != value)
                {
                    _isSaveEnabled = value;
                    OnPropertyChanged(nameof(IsSaveEnabled));
                }
            }
        }
        // 函数
        void ExecuteSearchFriCommand()
        {
            // 查找这个人是否存在
            var search = userRepository.Search(Model_user);
            if (search)
            {
                 
            }
            else
            {
                
                // 弹出“未找到”
                 
            }
           
        }
    }
}
