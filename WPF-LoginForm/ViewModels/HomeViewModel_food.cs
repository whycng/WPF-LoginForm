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
 

    public class HomeViewModel_food : ViewModelBase
    {
        private IItemRepo itemRepo;
        // public ObservableCollection<HomeModel_data_bh> data_bh { get; set; }
        public ObservableCollection<ItemModel> data_bh { get; set; }

        public
        void LoadDataBh()
        {
            var data_item = itemRepo.GetByClassify("food");
            data_bh = new ObservableCollection<ItemModel>(data_item);

            OnPropertyChanged("data_bh");

        }
        public HomeViewModel_food()
        {
            itemRepo = new ItemRepository();
            LoadDataBh();
        }
    }

}
