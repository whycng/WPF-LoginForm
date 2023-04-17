using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;

namespace WPF_LoginForm.ViewModels
{
    public class DetailsViewModel : ViewModelBase
    {
        public DetailsRepository detailsRepository;
        //private DetailsModel tmp;
        public DetailsModel _model_details;
        public DetailsModel Model_details
        {
            get { 
                return _model_details;
            } 
            set { 
                _model_details = value;
                // OnPropertyChanged("Model_details");
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
        public DetailsViewModel()
        {
            // Model_details = DetailsModelTmp;// tmp也不行
            detailsRepository = new DetailsRepository();
            Model_details = detailsRepository.GetDetails(detailsRepository.TmpGet());
        }


    }
}
