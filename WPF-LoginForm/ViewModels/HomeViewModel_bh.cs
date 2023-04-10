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
    public class HomeViewModel_bh:ViewModelBase
    {
        private IItemRepo itemRepo;
        // public ObservableCollection<HomeModel_data_bh> data_bh { get; set; }
        public ObservableCollection<ItemModel> data_bh { get; set; }

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
            itemRepo = new ItemRepository();
            LoadDataBh();
        }
    }
}
