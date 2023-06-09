﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using GalaSoft.MvvmLight.Command;
using LiveCharts.Wpf;
using LiveCharts;
using WPF_LoginForm.Views;

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
            AddItem = new RelayCommand(() => ExecuteAddItem());
            isMerchant = 0;// 不是

            LoadMerchantData();
            //Init2();
        }

        // 属性
        private int isMerchant;//是不是商家
        private IItemRepo itemRepo;
        private IUserRepository userRepository;
        public UserModel UserNow;
        private int idNow = -1;
        public List<int> AmountChange = new List<int>();
        public string PriceChange;
        private SellerModel _seller;
        public SellerModel Seller
        {
            get { return _seller; }
            set { _seller = value;
                OnPropertyChanged("Seller");
            }
        }
        //private int _totalOrder;// 成交量
        //public int TotalOrder
        //{
        //    get { return _totalOrder; }
        //    set { _totalOrder = value;
        //        OnPropertyChanged("TotalOrder");
        //    }    
        //}
        //private int _watch;// 查看量
        //public int Watch
        //{
        //    get { return _watch; }
        //    set
        //    {
        //        _watch = value;
        //        OnPropertyChanged("Watch");
        //    }
        //}
        //private double _revenue;// 总收入
        //public double Revenue
        //{
        //    get { return _revenue; }
        //    set
        //    {
        //        _revenue = value;
        //        OnPropertyChanged("Revenue");
        //    }
        //}
        //private int _appeal;// 申诉数
        //public int Appeal
        //{
        //    get { return _appeal; }
        //    set
        //    {
        //        _appeal = value;
        //        OnPropertyChanged("Appeal");
        //    }
        //}
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
        public RelayCommand AddItem { get; set; }

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
        public void ExecuteAddItem() // 新增商品
        {
            var model = new ItemModel();
            // 卖家默认填入
            // ...
            AddItemView view = new AddItemView();
            var r = view.ShowDialog(); //返回值r就是 【确定/取消】的结果
            if (r.Value)
            { // 点击了【确定】

            }
            else
            {
                // ...
            }
        }
        // 获取商家四项信息
        void GetTotalOrder()
        {
            Seller = itemRepo.GetSellerModelBySellername(UserNow.Username);
            //// 总成交量 --> 历史订单操作
            //TotalOrder = itemRepo.GetTotalOrderBySellername(UserNow.Username);
            //// 总访问量  --> 详情每点击一下
            //Watch = itemRepo.GetWatchBySellername(UserNow.Username);
            //// 总收入 --> 历史订单操作
            //Revenue = itemRepo.GetRevenueBySellername(UserNow.Username);
            //// 总申诉 -->   
            //Appeal = itemRepo.GetAppealBySellerName(UserNow.Username);
        }

        void LoadMerchantData() // 加载商家数据
        {
            // 取一下当前用户名
           
            // 如果你是商家，则显示
            if (itemRepo.IsMerchant(UserNow.Username))
            {
                isMerchant = 1;
                var t = itemRepo.GetBySellerName(UserNow.Username);
                
                data_item = new ObservableCollection<ItemModel>(t);
                GetTotalOrder();
                //Init2();
            }
            else
            {
                System.Windows.MessageBox.Show("你不是商家");
                // 你不是商家
            }

        }

        // 柱状图
        #region 属性
        private SeriesCollection achievement = new SeriesCollection();
        /// <summary>
        /// 成绩柱状图
        /// </summary>
        public SeriesCollection Achievement
        {
            get { return achievement; }
            set { achievement = value; }
        }
        private SeriesCollection _salesVolume = new SeriesCollection();
        /// <summary>
        /// 销售额柱状图
        /// </summary>
        public SeriesCollection SalesVolume
        {
            get { return _salesVolume; }
            set { _salesVolume = value; }
        }

        private List<string> studentNameList;
        /// <summary>
        /// 学生名字
        /// </summary>
        public List<string> StudentNameList
        {
            get { return studentNameList; }
            set { studentNameList = value; }
        }

        private List<string> _monthList;
        /// <summary>
        /// 月份
        /// </summary>
        public List<string> MonthList
        {
            get { return _monthList; }
            set { _monthList = value; }
        }

        #endregion


        #region 方法
        /// <summary>
        /// 成绩初始化
        /// </summary>
        public void Init()
        {
            //名字集合
            StudentNameList = new List<string>()
            {
                "张三",
                "李四",
                "王五",
                "赵六",
                "小明",
            };
            //成绩分数集合
            ChartValues<double> achievement = new ChartValues<double>();
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                achievement.Add(random.Next(60, 100));
            }
            var column = new ColumnSeries();
            column.DataLabels = true;
            column.Title = "成绩";
            column.Values = achievement;
            Achievement.Add(column);
 
        }

        public void Init2()
        {

            // 月份
            MonthList = new List<string>();
            for(int i = 1; i <= 12; i++)
            {
                string t = i.ToString() + "月"; 
                MonthList.Add(t);
            }
   
            // 销售额集合
            ChartValues<int> _salesVolume = new ChartValues<int>();
            Random random = new Random();
            // 拿数据 fun(int i)
            for(int i = 1; i <= 4 ; i++) // 暂时给出前四个月的数据
            {
                //if (isMerchant == 1)
                //    _salesVolume.Add(random.Next(0, 30));
                //else
                //    _salesVolume.Add(0);

                _salesVolume.Add(random.Next(0, 30));
            }
            // 拿到当月数据
            // ....
            //List<int> myList = new List<int>() { 10, 20, 30, 40 };
            //_salesVolume.AddRange(myList);

            SalesVolume.Clear(); // 清空 SalesVolume 中的柱状图
            var column = new ColumnSeries();
            column.DataLabels = true;
            column.Title = "销售额";
            column.Values = _salesVolume;
            SalesVolume.Add(column);
        }
        #endregion

    }


}
