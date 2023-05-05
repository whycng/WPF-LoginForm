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
    public class AddStoreViewModel:ViewModelBase
    {
        //public AddStoreViewModel(int itemId) {
        //    _itemId = itemId;
        //}
        public AddStoreViewModel()
        {
             
            itemRepo = new ItemRepository();
            userRepository = new UserRepository();
            data_store = new ObservableCollection<ItemModel>();
            Sellername = itemRepo.TmpGetSellername();
            LoadData();
        }
        // 属性 data_store
        // private int _itemId;
        private IItemRepo itemRepo;
        private IUserRepository userRepository;
        //public string _sellername;
        public string Sellername { get; set; }
  
        private ObservableCollection<ItemModel> _data_store;
        public ObservableCollection<ItemModel> data_store
        {
            get { return _data_store; }
            set
            {
                _data_store = value;
                OnPropertyChanged("data_store");
            }
        }
        // 命令
        private RelayCommand<int> _buyCommand;
        public RelayCommand<int> BuyCommand // int 传进了商品id
        {
            get
            {
                if (_buyCommand == null)
                    _buyCommand = new RelayCommand<int>((parameter) => ExcuteBuyCommand(parameter));
                return _buyCommand;
            }
        }
        private RelayCommand<int> _detailsCommand;
        public RelayCommand<int> DetailsCommand // 应该传进id
        {
            get
            {
                if (_detailsCommand == null)
                    _detailsCommand = new RelayCommand<int>((parameter) => ExcuteDetailsCommand(parameter));
                return _detailsCommand;
            }
        }
        // 函数
        private void ExcuteDetailsCommand(object parameter)
        {
            int ItemId = (int)parameter;// 加入购物车的商品id 
            //base.DetailsModelTmp = detailsRepository.GetDetails(ItemId);
            //GetInstace();
            AddDetails view = new AddDetails(); 
            var r = view.ShowDialog();
        }
        private void ExcuteBuyCommand(object parameter)
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            int ItemId = (int)parameter;// 加入购物车的商品id
            itemRepo.SetCart(ItemId, user.Username);

        }
        void LoadData()
        {
            var t = itemRepo.GetBySellerName(Sellername);
            data_store = new ObservableCollection<ItemModel>(t);
        }
    }
}
