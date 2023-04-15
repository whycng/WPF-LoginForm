using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using System.Xml.Linq;

namespace WPF_LoginForm.ViewModels
{
    public class CustomerViewModel : ViewModelBase // 用户
    {
        //Fields 字段
        private Uri _userPhoto;
        private IUserRepository userRepository;
        public UserModel User { get; set; } // 当前登录用户

        // Command UploadHeadImageCommand 上传用户头像
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

        private bool CanUploadImage()
        {
            // 在这里判断是否允许上传图片
            return true;
        }

        private void UploadImage()
        {
            // 在这里实现上传图片的逻辑

            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Image Files (*.png;*.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg; *.jpeg; *.gif; *.bmp";

            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //{
            //    //string serverUrl = "http://yourserver.com/upload";
            //    //string serverUrl = "F:\\abc\\毕设\\项目\\Login-In-WPF-MVVM-C-Sharp-and-SQL-Server-main\\ligub2\\loogin2\\WPF-LoginForm\\Assets\\userHead\\";
            //    string serverUrl = @"F:\\abc";
            //    string filePath = openFileDialog.FileName;

            //    WebClient client = new WebClient();

            //    client.UploadFile(serverUrl, filePath);

            //    MessageBox.Show("上传成功！");
            //}

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg; *.jpeg; *.gif; *.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string serverUrl = @"F:\\abc\\毕设\\项目\\Login-In-WPF-MVVM-C-Sharp-and-SQL-Server-main\\ligub2\\loogin2\\WPF-LoginForm\\Assets\\userHead\\"; // 保存文件的本地路径
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);

                try
                {
                    WebClient client = new WebClient();

                    client.UploadFile(serverUrl + Path.GetFileName(filePath), filePath); // 上传文件到本地路径
                    System.Windows.MessageBox.Show("上传成功！");

                    //如果上传成功，文件名更新到数据库
                    //1.先拿到当前用户名
                    // var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
                    if (User != null)//user.Username
                    {
                        userRepository.SetUserPhoto(User.Username,fileName);
                        UserPhoto = User.UserPhoto;//加载
                    }
                    else
                    {
                        throw new Exception("user is null");
                    }
                }
                catch (WebException ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }



        }
        private string _textBoxName;
        public string TextBoxName // name
        {
            get { return _textBoxName; }
            set
            {
                if (_textBoxName != value)
                {
                    _textBoxName = value;
                    OnPropertyChanged(nameof(TextBoxName));
                }
            }
        }
        public ICommand ConfirmNameCommand => new RelayCommand(() =>
        {
            string name = TextBoxName; // 获取Name属性的值 
            userRepository.SetNameByUserName(User.Username, name);

        });



        //Properties

        public Uri UserPhoto
        {
            get => _userPhoto;
            set
            {
                _userPhoto = value;
                OnPropertyChanged(nameof(UserPhoto));
            }//=> _testusername = value;
        }


        // -------------
        public CustomerViewModel() // 构造函数
        {
            userRepository = new UserRepository();
            User = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            // 加载数据
            UserPhoto = User.UserPhoto; 

        }
 
    }
}
