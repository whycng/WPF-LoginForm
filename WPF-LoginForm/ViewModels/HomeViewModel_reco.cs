﻿using GalaSoft.MvvmLight.Command;
using Panuon.WPF.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using WPF_LoginForm.Views;
using System.Windows.Controls.Primitives;



namespace WPF_LoginForm.ViewModels
{
  
    public class HomeViewModel_reco : ViewModelBase
    {
        private IUserRepository userRepository;
        private IItemRepo itemRepo;
        private DetailsRepository detailsRepository;// 未用接口
        // public ObservableCollection<HomeModel_data_bh> data_bh { get; set; }
        public ObservableCollection<ItemModel> data_bh { get; set; }
        private ObservableCollection<int> _quantities;
        public ObservableCollection<int> Quantities
        {
            get { 
                return _quantities; 
            }
            set
            {
                if (_quantities != value)
                {
                    _quantities = value;
                    OnPropertyChanged(nameof(Quantities));
                }
            }
        }
        


        //#region CheckBox
        //private String checkInfo; 

        //private RelayCommand checkCommand;

        //private List<CompBottonModel> checkButtons; 
        //public void loadCheck()
        //{
        //    CheckButtons = new List<CompBottonModel>();
        //    foreach (var ll in data_bh)
        //    {
        //        CheckButtons.Add(new CompBottonModel() { Id=ll.Id, IsCheck=false });
        //    } 
        //}
        //public List<CompBottonModel> CheckButtons
        //{
        //    get { return checkButtons; }
        //    set
        //    {
        //        checkButtons = value; // OnPropertyChanged("CheckButtons"); 
        //    }
        //}


        //public String CheckInfo
        //{
        //    get { return checkInfo; }
        //    set { checkInfo = value; OnPropertyChanged("CheckInfo"); }
        //} 
        //public RelayCommand CheckCommand
        //{
        //    get
        //    {
        //        if (checkCommand == null)
        //            checkCommand = new RelayCommand(() => ExcuteCheckCommand());
        //        return checkCommand;

        //    }
        //    set { checkCommand = value; }
        //}

        //private void ExcuteCheckCommand()
        //{
        //    CheckInfo = "";
        //    if (CheckButtons != null && CheckButtons.Count > 0)
        //    {
        //        var list = CheckButtons.Where(p => p.IsCheck);
        //        if (list.Count() > 0)
        //        {
        //            foreach (var l in list)
        //            {
        //                CheckInfo += l.Id + ",";
        //            }

        //            CheckInfo = CheckInfo.TrimEnd(',');  // 把最后一个逗号删掉
        //        }
        //        else
        //        {
        //            var x = "wrong";
        //        }
        //    }
        //}
        //#endregion

        //public RelayCommand<string> BuyCommand
        //{
        //    get
        //    {
        //        if (BuyCommand == null)
        //            BuyCommand = new RelayCommand<string>((parameter) => ExcuteBuyCommand(parameter));
        //        return BuyCommand;

        //    }
        //    set
        //    {
        //        BuyCommand = value;
        //    }
        //}

        #region Command
        private RelayCommand<int> _buyCommand;
        private RelayCommand<int>  _detailsCommand;
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

            //var model = detailsRepository.GetDetails(ItemId);
            // AddDetails view = new AddDetails(model);//id 应该传进来
            var r = view.ShowDialog();
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
            itemRepo.SetCart(ItemId, user.Username);
            
        }
        // public ICommand Buy(object par) => new RelayCommand(AddToCart);

        //public void Buy(object buttonName)
        //{
        //    string name = buttonName.ToString();
        //    // Button button = FindAncestor<Button>(this);
        //    // throw new NotImplementedException();
        //}

        //private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        //{
        //    while (current != null && !(current is T))
        //    {
        //        current = VisualTreeHelper.GetParent(current);
        //    }
        //    return current as T;
        //}
        #endregion

        public void LoadDataBh()
        {
            var data_item = itemRepo.GetBySellerName("admin");
            data_bh = new ObservableCollection<ItemModel>(data_item);
 
            OnPropertyChanged("data_bh");

        }
        public HomeViewModel_reco()
        {
            userRepository = new UserRepository();
            itemRepo = new ItemRepository();
            detailsRepository = new DetailsRepository();
            LoadDataBh();
            // loadCheck();
            Quantities = new ObservableCollection<int>();
        }
    }
}
