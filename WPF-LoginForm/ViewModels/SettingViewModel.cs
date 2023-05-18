using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using LiveCharts.Wpf;
using LiveCharts;
using WPF_LoginForm.Models;
using WPF_LoginForm.Views;
using System.Windows.Media;

namespace WPF_LoginForm.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        public SettingViewModel() {
            Init();

            #region 测试2
            //SeriesCollection = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Series 1",
            //        Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
            //    },
            //    new LineSeries
            //    {
            //        Title = "Series 2",
            //        Values = new ChartValues<double> { 6, 7, 3, 4 ,6 },
            //        PointGeometry = null
            //    },
            //    new LineSeries
            //    {
            //        Title = "Series 3",
            //        Values = new ChartValues<double> { 4,2,7,2,7 },
            //        PointGeometry = DefaultGeometries.Square,
            //        PointGeometrySize = 15
            //    }
            //};

            //Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May" };
            //YFormatter = value => value.ToString("C");

            ////modifying the series collection will animate and update the chart
            //SeriesCollection.Add(new LineSeries
            //{
            //    Title = "Series 4",
            //    Values = new ChartValues<double> { 5, 3, 2, 4 },
            //    LineSmoothness = 0, //0: straight lines, 1: really smooth lines
            //    PointGeometry = Geometry.Parse("m 25 70.36218 20 -28 -20 22 -8 -6 z"),
            //    PointGeometrySize = 50,
            //    PointForeground = Brushes.Gray
            //});

            ////modifying any series values will also animate and update the chart
            //SeriesCollection[3].Values.Add(5d);
            #endregion
        }
        //属性
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }
        //命令
        private RelayCommand _testStoreCommand;
        public RelayCommand TestStoreCommand
        {
            get
            {
                if (_testStoreCommand == null)
                    _testStoreCommand = new RelayCommand(() => ExecuteTestStoreCommand());
                return _testStoreCommand;
            }
        }
        //函数
        void ExecuteTestStoreCommand()
        { 
            StoreView view = new StoreView();
            var r = view.ShowDialog();
        }


        //
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
        private SeriesCollection achievement2 = new SeriesCollection();
        /// <summary>
        /// 成绩柱状图
        /// </summary>
        public SeriesCollection Achievement2
        {
            get { return achievement2; }
            set { achievement2 = value; }
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

            // 测试用例
            ChartValues<double> achievement2 = new ChartValues<double>();
            Random random2 = new Random();
            for (int i = 0; i < 5; i++)
            {
                achievement.Add(random.Next(60, 100));
            }
            var column2 = new ColumnSeries();
            column2.DataLabels = true;
            column2.Title = "成绩";
            column2.Values = achievement;
            Achievement2.Add(column2);
        }
        #endregion

    }
}