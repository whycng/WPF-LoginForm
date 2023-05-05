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
            DefineAmountCommand = new RelayCommand<string>(t => DefineAmount(t));
            DefineProceCommand = new RelayCommand<string>(t => DefineProce(t));
            ChooseCommand = new RelayCommand<int>(t => Choose(t));


            LoadMerchantData();
        }

        // 属性
        private IItemRepo itemRepo;
        private IUserRepository userRepository;
        public UserModel UserNow;
        private int idNow = -1;
        public List<int> AmountChange = new List<int>();
        public string PriceChange;
        private ObservableCollection<ItemModel> _data_item;
        public ObservableCollection<ItemModel> data_item
        { 
            get { return _data_item; }
            set { _data_item = value;
                OnPropertyChanged("data_item");
            }
        }
        // 命令
        public RelayCommand<int> DelCommand { get; set; }
        public RelayCommand<string> DefineAmountCommand { get; set; }
        public RelayCommand<string> DefineProceCommand { get; set; }
        public RelayCommand<int> ChooseCommand { get; set; }

        //DefineAmount DefineProce  Choose
        public void Choose(int id)
        {
            idNow = id; 
        }
        public void DefineAmount(string amount)
        {
            int x;
            if(int.TryParse(amount, out x))
            {
                itemRepo.SetAmountById(idNow,x);
            }
            else
            {
                // 弹出输入不是int
                // 不需要了，见 PreviewTextInput="HandlePreviewTextInput，System.Windows.MessageBox.Show("不是int");
            }

            LoadMerchantData();
        }
        public void DefineProce(string price)
        { 
            itemRepo.SetPriceById(idNow,price);
            LoadMerchantData();
        }
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
                System.Windows.MessageBox.Show("你不是商家");
                // 你不是商家
            }

        }
    }


}
