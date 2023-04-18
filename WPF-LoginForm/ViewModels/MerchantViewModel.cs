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



namespace WPF_LoginForm.ViewModels
{
    public class MerchantViewModel:ViewModelBase
    {
        public MerchantViewModel() {

            itemRepo = new ItemRepository();
            userRepository = new UserRepository();
            UserNow = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            DelCommand = new RelayCommand<int>(t => Del(t));
            DefineAmountCommand = new RelayCommand<int>(t => DefineAmount(t));
            DefineProceCommand = new RelayCommand<int>(t => DefineProce(t));


            LoadMerchantData();
        }

        // 属性
        private IItemRepo itemRepo;
        private IUserRepository userRepository;
        public UserModel UserNow;
        public ObservableCollection<ItemModel> data_item { get; set; }
        // 命令
        public RelayCommand<int> DelCommand { get; set; }
        public RelayCommand<int> DefineAmountCommand { get; set; }
        public RelayCommand<int> DefineProceCommand { get; set; }


        public void Del(int id)
        {
            itemRepo.DelItemById(id); 
            LoadMerchantData(); 
        }
        void LoadMerchantData() // 加载商家数据
        {
            // 取一下当前用户名
           
            // 如果你是商家，则显示
            if (itemRepo.IsMerchant(UserNow.Username))
            {
                var t = itemRepo.GetBySellerName(UserNow.Username); 
                data_item = new ObservableCollection<ItemModel>(t);
            }
            else
            {
                // 你不是商家
            }

        }
    }


}
