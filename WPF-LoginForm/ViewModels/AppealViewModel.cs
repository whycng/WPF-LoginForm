using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using GalaSoft.MvvmLight.Command;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class AppealViewModel:ViewModelBase
    {
        public AppealViewModel() {
            itemRepo = new ItemRepository();
            userRepository = new UserRepository();
            UserNow = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);

            ReplyCommand = new RelayCommand<int>(t => Reply(t));

            // 加载数据
            LoadAppealData();
        }
        // 命令 ReplyCommand
        public RelayCommand<int> ReplyCommand { get; set; }

        void Reply(int ItemId)
        {
            // 打开回复页面
            var model = new AppealModel();
            AddReply view = new AddReply(model);
            var r = view.ShowDialog();  
            if (r.Value)
            {
                itemRepo.UpdataAppealReplyByItemId(ItemId , model.SellerReply);
                System.Windows.MessageBox.Show("回复成功");
            }
            else
            {
                // 取消
            }
              //  throw new NotImplementedException();
            LoadAppealData();
        }
        // 属性
        private string _showText;
        private int _showFontSize;
        private string _showForeground;

        private IItemRepo itemRepo; // 
       
        private IUserRepository userRepository;
        public UserModel UserNow;
        private ObservableCollection<AppealModel> _data_appeal;
        public string ShowForeground
        {
            get { return _showForeground; }
            set
            {
                _showForeground = value;
                OnPropertyChanged("ShowForeground");
            }
        }
        public string ShowText
        {
            get { return _showText; }
            set { _showText = value;
                OnPropertyChanged("ShowText");
            }
        }
        public int ShowFontSize
        {
            get { return _showFontSize; }
            set
            {
                _showFontSize = value;
                OnPropertyChanged("ShowFontSize");
            }
        }
        public ObservableCollection<AppealModel> data_appeal
        {
            get { return _data_appeal; }
            set
            {
                _data_appeal = value;
                OnPropertyChanged("data_appeal");
            }
        }
        //命令

        void LoadAppealData()
        {
            // 取一下当前用户名

            // 如果你是商家，则显示商家处理申诉
            if (itemRepo.IsMerchant(UserNow.Username))
            {
                var t = itemRepo.GetAppealBySellerName(UserNow.Username);
                ShowText = "你好，" + UserNow.Username;
                ShowFontSize = 15;
                ShowForeground = "#784DFD";
                data_appeal = new ObservableCollection<AppealModel>(t);
          
            }
            else
            {
                // 你不是商家 
                ShowText = "你不是商家";
                ShowFontSize = 50;
                ShowForeground = "red";
            }

        }
    }
}
