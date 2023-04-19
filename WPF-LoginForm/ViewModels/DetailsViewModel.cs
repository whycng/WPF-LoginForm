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
        private IItemRepo itemRepo;
        int CollageItemId = -1;
        public DetailsRepository detailsRepository;
        public UserRepository userRepository;
        //private DetailsModel tmp;
        // CollageCommand 
        private RelayCommand<int> _collageCommand;//拼单命令
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
        }// ExcuteCommentHereCommand

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
        #region Collage 拼单
        public ObservableCollection<CollageModel> data_collage { get; set; }
        public void LoadDataCollage()
        {
            var data_item = detailsRepository.GetCollage();
            data_collage = new ObservableCollection<CollageModel>(data_item);

            OnPropertyChanged("data_bh");

        }
        #endregion
        public DetailsModel Model_details
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

        void LoadData()
        {
            Model_details = detailsRepository.GetDetails(detailsRepository.TmpGet());

        }
        public DetailsViewModel()
        {
            itemRepo = new ItemRepository();
            // Model_details = DetailsModelTmp;// tmp也不行
            detailsRepository = new DetailsRepository();
            userRepository = new UserRepository();
            Model_details = detailsRepository.GetDetails(detailsRepository.TmpGet());

            CommentHereCommand = new RelayCommand<string>(t => CommentHere(t));

            //拼单
            LoadDataCollage();
        }


    }
}
