using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace WPF_LoginForm.ViewModels
{
    public class AddItemViewModel : ViewModelBase
    {
        public AddItemViewModel() {
            
            // 信息初始化 
            ModelItem = new ItemModel();
            userRepository = new UserRepository();
            itemRepository = new ItemRepository();
            UserNow = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            ConfirmCommand = new RelayCommand(() => ExecuteConfirmCommand());
        }
        // 命令
        public RelayCommand ConfirmCommand { get; set; }
        private ICommand _uploadImageCommand;

        public ICommand UploadImageCommand
        {
            get
            {
                if (_uploadImageCommand == null)
                {
                    _uploadImageCommand = new RelayCommand(UploadImage, CanUploadImage);
                }

                return _uploadImageCommand;
            }
        }
        // 属性
        string photoName;
        public UserModel UserNow;
        private IUserRepository userRepository;
        private IItemRepo itemRepository;
        private ItemModel _modelItem;// 添加的商品
        public ItemModel ModelItem
        {
            get { return _modelItem; }
            set
            {
                _modelItem = value;
                OnPropertyChanged("ModelItem");
            }
        }
        // 函数
        void ExecuteConfirmCommand()
        {
            // 将 ModelItem 添加到 Item表
             
            ModelItem.SellerName = UserNow.Username;
            //Amount Date reco 
            //ModelItem.Date = "";
            //ModelItem.reco = ""; 
            itemRepository.InsertItem(ModelItem, photoName);
        }

        private bool CanUploadImage()
        {
            // 在这里判断是否允许上传图片
            return true;
        }

        private void UploadImage()
        {
  
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg; *.jpeg; *.gif; *.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string serverUrl = @"F:\\abc\\毕设\\项目\\Login-In-WPF-MVVM-C-Sharp-and-SQL-Server-main\\ligub2\\loogin2\\WPF-LoginForm\\Assets\\item\\"; // 保存文件的本地路径
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);

                try
                {
                    WebClient client = new WebClient();

                    client.UploadFile(serverUrl + Path.GetFileName(filePath), filePath); // 上传文件到本地路径
                    System.Windows.MessageBox.Show("上传成功！");

                    //userRepository.SetUserPhoto(User.Username, fileName);
                    //UserPhoto = User.UserPhoto; 

                    ModelItem.ItemPhoto = new Uri("/assets/item/" + fileName, UriKind.RelativeOrAbsolute);
                    photoName = fileName.Replace(" ","");


      //ItemPhotoStr = new Uri("/assets/item/" + ModelIte., UriKind.RelativeOrAbsolute),


                }
                catch (WebException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }



        }

    }
}
