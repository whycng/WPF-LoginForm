using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using GalaSoft.MvvmLight.Command;
using WPF_LoginForm.Views;
using System.Collections.ObjectModel;
using System.Threading;

namespace WPF_LoginForm.ViewModels
{
    public class DetailsViewModel : ViewModelBase
    {
        public DetailsViewModel()
        {
            itemRepo = new ItemRepository();
            // Model_details = DetailsModelTmp;// tmp也不行
            detailsRepository = new DetailsRepository();
            userRepository = new UserRepository();
            Model_details = detailsRepository.GetDetails(detailsRepository.TmpGet());

            CommentHereCommand = new RelayCommand<string>(t => CommentHere(t));
            // 未知错误
            //StoreCommand = new GalaSoft.MvvmLight.Command.RelayCommand(ExecuteStoreCommand);

            //拼单
            LoadDataCollage();
        }

        // 属性
        private IItemRepo itemRepo;
        int CollageItemId = -1;
        public DetailsRepository detailsRepository;
        public UserRepository userRepository;
         
        // 命令 StoreCommand
        // public GalaSoft.MvvmLight.Command.RelayCommand StoreCommand; // 打开商店
        private RelayCommand<int> _collageCommand;//拼单命令
        private RelayCommand  _storeCommand; 
        public DetailsModel _model_details; // CommentHereCommand

        public RelayCommand<string> CommentHereCommand { get; set; }

        public RelayCommand<int> CollageCommand // int 传进了商品id
        {
            get
            {
                if (_collageCommand == null)
                    _collageCommand = new RelayCommand<int>((parameter) => ExcuteCollageCommand(parameter));
                return _collageCommand;
            }
        }
        public RelayCommand StoreCommand
        {
            get
            {
                if (_storeCommand == null)
                    _storeCommand = new RelayCommand(() => ExecuteStoreCommand());
                return _storeCommand;
            }
        }

        #region Collage 拼单
        public ObservableCollection<CollageModel> data_collage { get; set; }
        public void LoadDataCollage()
        {
            var data_item = detailsRepository.GetCollage();
            data_collage = new ObservableCollection<CollageModel>(data_item);

            OnPropertyChanged("data_bh");

        }
        #endregion
        public DetailsModel Model_details // 当前页面的所以信息，商品商店商家等
        {
            get { 
                return _model_details;
            } 
            set { 
                _model_details = value;
                OnPropertyChanged("Model_details");
            }

        }
        public DetailsViewModel(DetailsModel model)
        {
            Model_details = model;
            // tmp = model;//不知道为啥，tmp的数据没用存储下来
            // suoyi先存入数据库
            detailsRepository = new DetailsRepository();
            detailsRepository.TmpSet(model.item.Id);
        }

        // 函数

        void ExecuteStoreCommand()
        {
            // 打开商店
            // AddStoreViewModel viewModel = new AddStoreViewModel(Model_details.item.Id);
            // 因为错误，更换解决方法，使用临时表，卖家名
            itemRepo.TmpSetSellername(Model_details.item.SellerName);
            AddStoreView view = new AddStoreView();
            var r = view.ShowDialog();
        }
        void LoadData()
        {
            Model_details = detailsRepository.GetDetails(detailsRepository.TmpGet());

        }

        public void CommentHere(string Comment)// 评论
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            itemRepo.SetCommentByUser(Model_details.item.Id, user.Username, Comment);
            LoadData();
        }

        void ExcuteCollageCommand(object parameter)
        {
            CollageItemId = (int)parameter;

            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);

            // 写入拼单表
            detailsRepository.SetCollageById(CollageItemId, user.Username);
            // 打开拼单页面  可以将其上下文设置在此
            AddCollage view = new AddCollage();
            var r = view.ShowDialog();
        }
    }
}
