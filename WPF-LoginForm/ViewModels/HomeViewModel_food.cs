using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using GalaSoft.MvvmLight.Command;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
 

    public class HomeViewModel_food : ViewModelBase
    {
        private IItemRepo itemRepo;
        // public ObservableCollection<HomeModel_data_bh> data_bh { get; set; }
        public ObservableCollection<ItemModel> data_bh { get; set; }


        #region Command
        private RelayCommand<int> _buyCommand;
        private RelayCommand _detailsCommand;
        public RelayCommand DetailsCommand // 应该传进id
        {
            get
            {
                if (_detailsCommand == null)
                    _detailsCommand = new RelayCommand(() => ExcuteDetailsCommand());
                return _detailsCommand;
            }
        }
        public RelayCommand<int> BuyCommand // int 传进了商品id
        {
            get
            {
                if (_buyCommand == null)
                    _buyCommand = new RelayCommand<int>((parameter) => ExcuteBuyCommand(parameter));
                return _buyCommand;
            }
        }
        private void ExcuteDetailsCommand()
        {
            AddDetails view = new AddDetails();
            var r = view.ShowDialog();
        }

        private void ExcuteBuyCommand(object parameter)
        {
            int ItemId = (int)parameter;// 加入购物车的商品id
            itemRepo.SetCart(ItemId);
        }

        #endregion

        public
        void LoadDataBh()
        {
            var data_item = itemRepo.GetByClassify("food");
            data_bh = new ObservableCollection<ItemModel>(data_item);

            OnPropertyChanged("data_bh");

        }
        public HomeViewModel_food()
        {
            itemRepo = new ItemRepository();
            LoadDataBh();
        }
    }

}
