using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;

namespace WPF_LoginForm.ViewModels
{
    public class AdminViewModel:ViewModelBase
    {
        public AdminViewModel() {
            userRepository = new UserRepository();
            itemRepo = new ItemRepository();



            LoadData();
        }
        // 属性
        public UserModel UserNow;
        private IUserRepository userRepository;
        private IItemRepo itemRepo;
        public ObservableCollection<UserModel> data_Seller { get; set; } // 卖家数据
        public ObservableCollection<UserModel> data_User { get; set; } // 普通用户数据
        public ObservableCollection<ItemModel> data_Item { get; set; } //商品数据  
        public ObservableCollection<AppealModel> data_Appeal { get; set; } //申诉数据  


        // 命令

        void LoadData()
        {
            // 加载卖家
            var t1 = userRepository.GetSeller();
            data_User = new ObservableCollection<UserModel>(t1);
            OnPropertyChanged("data_User");
            // 加载普通用户
            var t2 = userRepository.GetUser();
            data_Seller = new ObservableCollection<UserModel>(t1);
            OnPropertyChanged("data_Seller");
            // 加载商品数据
            var t3 = itemRepo.GetItemAll();
            data_Item = new ObservableCollection<ItemModel>(t3);
            OnPropertyChanged("data_Item");
            // 加载申诉  
            var t4 = itemRepo.GetAppealAll();
            data_Appeal = new ObservableCollection<AppealModel>(t4);
            OnPropertyChanged("data_Appeal");
        }
    }
}
