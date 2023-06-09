﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using WPF_LoginForm.Views;
using GalaSoft.MvvmLight.Command;
using System.Threading;

namespace WPF_LoginForm.ViewModels
{
    public class HomeViewModel_bh:ViewModelBase
    {
        private IUserRepository userRepository;
        private IItemRepo itemRepo;
        private DetailsRepository detailsRepository;
        // public ObservableCollection<HomeModel_data_bh> data_bh { get; set; }
        public ObservableCollection<ItemModel> data_bh { get; set; }

        #region Command
        private RelayCommand<int> _buyCommand;
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
        private void ExcuteDetailsCommand(object parameter)
        {
            int ItemId = (int)parameter;// 加入购物车的商品id 
            base.DetailsModelTmp = detailsRepository.GetDetails(ItemId);
            GetInstace();
            AddDetails view = new AddDetails();
            var r = view.ShowDialog();

            // 旧方法，关键在于DetailsViewModel的两个初始化函数
            //int ItemId = (int)parameter;// 加入购物车的商品id 
            //DetailsModelTmp = detailsRepository.GetDetails(ItemId);
            //AddDetails view = new AddDetails();
            //var r = view.ShowDialog();
        }
        public DetailsViewModel GetInstace()
        {
            return new DetailsViewModel(base.DetailsModelTmp);
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
 

        private void ExcuteBuyCommand(object parameter)
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            int ItemId = (int)parameter;// 加入购物车的商品id
            itemRepo.SetCart(ItemId,user.Username);
        }
 
        #endregion


        public
        void LoadDataBh()
        {
            var data_item = itemRepo.GetByClassify("bh");
            data_bh = new ObservableCollection<ItemModel>(data_item);
            //data_bh = new ObservableCollection<HomeModel_data_bh>()
            //{
            //    new HomeModel_data_bh()
            //    {
            //        ItemName="商品1",
            //        SellerName="upc",
            //        ItemShowText="很好商品",
            //        ItemPhoto=new Uri("/assets/bh1.jpg", UriKind.RelativeOrAbsolute),
            //    }, new HomeModel_data_bh()
            //    {
            //        ItemName="商品2",
            //        SellerName="upc",
            //        ItemShowText="很好商品",
            //        ItemPhoto=new Uri("/assets/bh1.jpg", UriKind.RelativeOrAbsolute),
            //    }, new HomeModel_data_bh()
            //    {
            //        ItemName="商品3",
            //        SellerName="upc",
            //        ItemShowText="很好商品",
            //        ItemPhoto=new Uri("/assets/bh1.jpg", UriKind.RelativeOrAbsolute),
            //    }, new HomeModel_data_bh()
            //    {
            //        ItemName="商品4",
            //        SellerName="upc",
            //        ItemShowText="很好商品",
            //        ItemPhoto=new Uri("/assets/bh1.jpg", UriKind.RelativeOrAbsolute),
            //    }, new HomeModel_data_bh()
            //    {
            //        ItemName="商品5",
            //        SellerName="upc",
            //        ItemShowText="很好商品",
            //        ItemPhoto=new Uri("/assets/bh1.jpg", UriKind.RelativeOrAbsolute),
            //    }, new HomeModel_data_bh()
            //    {
            //        ItemName="商品6",
            //        SellerName="upc",
            //        ItemShowText="很好商品",
            //        ItemPhoto=new Uri("/assets/bh1.jpg", UriKind.RelativeOrAbsolute),
            //    }, new HomeModel_data_bh()
            //    {
            //        ItemName="商品1",
            //        SellerName="upc",
            //        ItemShowText="很好商品",
            //        ItemPhoto=new Uri("/assets/bh1.jpg", UriKind.RelativeOrAbsolute),
            //    },



            //};
            OnPropertyChanged("data_bh");

        }
        public HomeViewModel_bh()
        {
            userRepository = new UserRepository();
            itemRepo = new ItemRepository();
            detailsRepository = new DetailsRepository();
            LoadDataBh();
        }
    }
}
