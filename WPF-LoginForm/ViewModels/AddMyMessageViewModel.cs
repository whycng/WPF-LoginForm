using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;

namespace WPF_LoginForm.ViewModels
{
    public class AddMyMessageViewModel:ViewModelBase
    {
        public AddMyMessageViewModel() {

            // 初始化
            AcceptCommand = new RelayCommand<string>(t => ExecuteAcceptCommand(t));
            RejectCommand = new RelayCommand<string>(t => ExecuteRejectCommand(t));


            userRepository = new UserRepository();
            UserNow = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            LoadData();
        }
        // 属性 AddFri 
        public UserModel UserNow;
        private ObservableCollection<AddFriModel> _addFri;
        public ObservableCollection<AddFriModel> AddFri
        {
            get { return _addFri; }
            set { _addFri = value;
                OnPropertyChanged("AddFri");
            }
        }
        private IUserRepository userRepository;
        // 命令 AcceptCommand RejectCommand
        public RelayCommand<string> AcceptCommand { get; set; }
        public RelayCommand<string> RejectCommand { get; set; }


        // 函数
        void ExecuteAcceptCommand(string FromUsername)
        {
            // 接受好友邀请
            //  加为好友
            userRepository.AddFriend(UserNow.Username, FromUsername);
        }
        void ExecuteRejectCommand(string FromUsername)
        {
            // 拒绝好友邀请
            // 发送回执
        }
        void LoadData()
        {
            var t = userRepository.GetAddFriByNowUsername(UserNow.Username);
            if(t.Count > 0)
            {
                AddFri = new ObservableCollection<AddFriModel>(t);
            }
            else // 空
            { 
                AddFri = new ObservableCollection<AddFriModel>(); 
            }  
            
        }
    }
}
