using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        }

        localDB localDB;
        private string search =string.Empty;
       
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
        #region Command

        public RelayCommand QueryCommand { get; set; }
        public RelayCommand ResetCommand { get; set; }
        public RelayCommand<int> EditCommand { get; set; }
        public RelayCommand<int> DelCommand { get; set; }

        public RelayCommand AddCommand { get; set; }

        #endregion


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

        public void Add()
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
    }
}