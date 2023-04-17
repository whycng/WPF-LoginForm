using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class CompassViewModel : ViewModelBase
    {
        public CompassViewModel() //初始化
        {
            #region Stu测试
            localDB = new localDB();
            QueryCommand = new RelayCommand(Query);
            ResetCommand = new RelayCommand(() => // 重置的明命令
            {
                Search = string.Empty; //搜索条件置为空
                this.Query(); // 即查询到所有数据
            });
            EditCommand = new RelayCommand<int>(t => Edit(t)); 
            DelCommand = new RelayCommand<int>(t => Del(t));
            AddCommand = new RelayCommand(Add);
            #endregion


            QueryHistCommand = new RelayCommand(QueryHist);
            ResetHistCommand = new RelayCommand(() => // 重置的明命令
            {
                SearchHist = string.Empty; //搜索条件置为空
                LoadHistData();
            });

            itemRepo = new ItemRepository();
            AddCommand_Cart = new RelayCommand<int>(t => Add_Cart(t));
            SubCommand_Cart = new RelayCommand<int>(t => Sub_Cart(t));
            DelCommand_Cart = new RelayCommand<int>(t => Del_Cart(t));
            DelCommand_Hist = new RelayCommand<int>(t => Del_Hist(t));//
            AppealCommand = new RelayCommand<int>(t => Appeal_Hist(t));//AppealCommand
            EmptyCartCommand = new RelayCommand(() => // 重置的明命令
            {
                itemRepo.EmptyCart();
                LoadCartData();
            });

            // 购物车处理 
            LoadCartData(); 
            // 历史订单处理
            LoadHistData();
        }



        #region 购物车，历史订单处理
        // AddCommand_Cart
        public RelayCommand<int> AddCommand_Cart { get; set; } // 个数加
        public RelayCommand<int> SubCommand_Cart { get; set; } // 个数减
        public RelayCommand<int> DelCommand_Cart { get; set; }
        public RelayCommand<int> DelCommand_Hist { get; set; }
        public RelayCommand<int> AppealCommand { get; set; }
        private IItemRepo itemRepo;
        // public ObservableCollection<HomeModel_data_bh> data_bh { get; set; }
        public ObservableCollection<ItemModel> data_cart { get; set; } // 购物车数据
        public ObservableCollection<ItemModel> data_histItem { get; set; } //历史订单数据  

        public RelayCommand QueryHistCommand { get; set; } 
        public RelayCommand ResetHistCommand { get; set; }
        public RelayCommand EmptyCartCommand { get; set; }
        // BuyCommand 
        private RelayCommand _buyCommand;
        private int _buyAmount = 0;
        private string searchHist = string.Empty;
        public RelayCommand BuyCommand // 购物车购买按钮
        {
            get
            {
                if (_buyCommand == null)
                    _buyCommand = new RelayCommand(() => ExcuteBuyCommand());
                return _buyCommand;
            }
        }


        public string SearchHist
        {
            get => searchHist;
            set
            {
                searchHist = value;
                OnPropertyChanged(nameof(SearchHist));
            } 
        }

        private void ExcuteBuyCommand()
        {
            // 1.正在发货 2.塞进历史订单
            // 扫描微信二维码
            Process.Start("https://i.328888.xyz/2023/04/17/iekViJ.png");
            // 如果购买成功 则：
            //2.直接把购物车塞进历史订单 
            itemRepo.SetHisOrd();
            LoadHistData();
        }

        void LoadHistData()// 历史订数据加载
        {
            // 不对，购买按钮执行：itemRepo.SetHisOrd();// 购物车数据放入历史订单
            var t = itemRepo.GetHisOrd();
            data_histItem = new ObservableCollection<ItemModel>(t);

            OnPropertyChanged("data_histItem");
        }
        public void LoadCartData() // 加载购物车数据
        {
            var data_item = itemRepo.GetCart();
            data_cart = new ObservableCollection<ItemModel>(data_item);

            OnPropertyChanged("data_cart");

        }

        public void Add_Cart(int id)  
        {
            itemRepo.AddCartById(id);
            // OnPropertyChanged("data_cart");   
            LoadCartData();

        }
        public void Sub_Cart(int id)
        {
            itemRepo.SubCartById(id);
            // OnPropertyChanged("data_cart");   
            LoadCartData();

        }
        public void Del_Cart(int id)
        {
            itemRepo.DelCartById(id);
            // OnPropertyChanged("data_cart");   
            LoadCartData();

        }
        public void Del_Hist(int id)
        {
            itemRepo.DelHistById(id);
            LoadHistData();
        }//
        public void Appeal_Hist(int id)
        {
            // 申诉逻辑...
            // 申诉逻辑...
            System.Windows.MessageBox.Show("申诉成功");
        }//Appeal_Hist

        public void QueryHist()
        {
            // var models = localDB.GetSutdentsByName(Search);
            if(String.IsNullOrEmpty(SearchHist)) 
            { LoadHistData(); }
            else
            { // 应该是一个模糊搜索
                var t = itemRepo.GetHisOrdByVague(SearchHist);
                data_histItem = new ObservableCollection<ItemModel>(t);
            } 
            OnPropertyChanged("data_histItem");
        }
        #endregion


        #region Stu测试
        #region Stu测试命令Command

        public RelayCommand QueryCommand { get; set; } 
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand<int> EditCommand { get; set; }
        public RelayCommand<int> DelCommand { get; set; }

        public RelayCommand AddCommand { get; set; }
        localDB localDB;
        private string search = string.Empty;

        private ObservableCollection<Sutdent> gridModelList;


        public ObservableCollection<Sutdent> GridModelList
        {
            get { return gridModelList; }
            set { gridModelList = value; OnPropertyChanged(nameof(gridModelList)); }
        }
    

        public string Search
        {
            get => search;
            set
            {
                search = value;
                OnPropertyChanged(nameof(Search));
            }// => search = value; }
        }

        #endregion

        #region Stu测试表
        public void Query()
        {
            var models = localDB.GetSutdentsByName(Search); //通过打断点发现，Search即为查找输入框内的内容
            // var models = localDB.GetSutdents();
            GridModelList = new ObservableCollection<Sutdent>();
            if(models != null)
            {
                models.ForEach(arg =>
                {
                    GridModelList.Add(arg);
                });
            }
        }
        
        public void Edit(int id)
        {
            var model = localDB.GetSutdentById(id);
            if(model != null)
            {
                AddStuView view = new AddStuView(model);
                var r = view.ShowDialog(); //返回值r就是 AddStuView页面【确定/取消】的结果
                if (r.Value)
                {
                    var newModel = GridModelList.FirstOrDefault(t => t.Id == model.Id);
                    if(newModel != null)
                    {
                        newModel.Name = model.Name; // 找到他并更新他的名称
                    }
                }
            }
        }

        public void Add() // 测试学生表
        {
            Sutdent student = new Sutdent();
            AddStuView view = new AddStuView(student);
            var r = view.ShowDialog(); //返回值r就是 AddStuView页面【确定/取消】的结果
            if (r.Value)
            {
                student.Id = GridModelList.Max(t => t.Id) + 1;
                localDB.AddStudnet(student);
                this.Query();
            }
        }

        public void Del(int id)
        {
            var model = localDB.GetSutdentById(id);
            if (model != null)
            {
                var r = MessageBox.Show($"确认删除当前用户:{model.Name}", "操作提示", MessageBoxButton.OK, MessageBoxImage.Question);
                if(r == MessageBoxResult.OK)
                {
                    localDB.DelStudent(model.Id);
                }
                this.Query();
            }

        }
        #endregion
        #endregion
    }
}