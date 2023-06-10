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
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;

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
        // ConfirmSexCommand

        private bool _isMale;


        private bool _isFemale;

        private string _textBoxName;
        private string _address;
        private string _phone;
        private string _email;
        private string _passwordX;
        //Properties
        public string PasswordX
        {
            get { return _passwordX; }
            set
            {
                if (_passwordX != value)
                {
                    _passwordX = value;
                    OnPropertyChanged(nameof(PasswordX));
                }
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                if (_address != value)
                {
                    _address = value;
                    OnPropertyChanged(nameof(Address));
                }
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (_phone != value)
                {
                    _phone = value;
                    OnPropertyChanged(nameof(Phone));
                }
            }
        }

        public bool IsMale
        {
            get { return _isMale; }
            set
            {
                if (_isMale != value)
                {
                    _isMale = value;
                    OnPropertyChanged(nameof(IsMale));
                }
            }
        }
        public bool IsFemale
        {
            get { return _isFemale; }
            set
            {
                if (_isFemale != value)
                {
                    _isFemale = value;
                    OnPropertyChanged(nameof(IsFemale));
                }
            }
        }
        public Uri UserPhoto
        {
            get => _userPhoto;
            set
            {
                _userPhoto = value;
                OnPropertyChanged(nameof(UserPhoto));
            }//=> _testusername = value;
        }
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
                        OnPropertyChanged(nameof(UserPhoto));
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

        public ICommand ConfirmPasswordCommand => new RelayCommand(() =>
        {
            var password = PasswordX; // 获取Name属性的值 
                                      // 判断密码强弱
            if (!Regex.IsMatch(password, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=_!])[a-zA-Z0-9@#$%^&+=_!]{8,}$")) // 判断密码强度
            { // 至少8-16个字符，至少1个大写字母，1个小写字母和1个数字，其他可以是任意字符：
                System.Windows.MessageBox.Show("密码必须包含至少一个数字、一个小写字母、一个大写字母以及一个特殊字符(@#\r\n%^&+=_!] 长度至少为8位。");
            }
            else
            {
                userRepository.SetPasswordByUserName(User.Username, password);
                System.Windows.MessageBox.Show("密码修改成功！");
            }
           

        });
        public ICommand ConfirmSexCommand => new RelayCommand(() => {
            var sex = IsFemale ? '女'.ToString() : '男'.ToString() ;
            userRepository.SetSexByUserName(User.Username, sex);

        });
        public ICommand ConfirmNameCommand => new RelayCommand(() =>
        {
            var name = TextBoxName; // 获取Name属性的值 
            userRepository.SetNameByUserName(User.Username, name);

        });
        public ICommand ConfirmAddressCommand => new RelayCommand(() =>
        {
            var address = Address;  
            userRepository.SetAddressByUserName(User.Username, address);

        }); 
        public ICommand ConfirmPhoneCommand => new RelayCommand(() =>
        {
            var phone = Phone;
            if (!Regex.IsMatch(phone, @"\d{3}-\d{8}|\d{4}-\{7,8}"))
            {// 手机号 校验

//                格式为 "XXX-XXXXXXXX" 的电话号码，其中 X 代表某个数字。
//                例如：021 - 12345678。

//格式为 "XXXX-XXXXXXXX" 或 "XXXX-XXXXXXXXX" 的电话号码，其中 X 代表某个数字。
//                例如：0571 - 1234567 或 0312 - 76543210。
                System.Windows.MessageBox.Show("手机号格式不对");
            }else
            userRepository.SetPhoneByUserName(User.Username, phone);

        });
        public ICommand ConfirmEmailCommand => new RelayCommand(() =>
        {
            var email = Email;
            if (!Regex.IsMatch(email, @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?")) {
                System.Windows.MessageBox.Show("邮箱格式不对");
            }else
            userRepository.SetEmailByUserName(User.Username, email);

        });



        // -------------
        public CustomerViewModel() // 构造函数
        {
            userRepository = new UserRepository();
            User = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            // 加载数据
            TextBoxName = User.Name;
            UserPhoto = User.UserPhoto;
            Address = User.Address;
            var sex = User.Sex.Replace(" ","");
            if (sex == '男'.ToString())
            { 
                IsMale = true; 
                IsFemale = false; 
            }
            else {
                IsFemale = true;
                IsMale = false;
            }
            Email = User.Email;
            Phone = User.Phone;
            // PasswordX = User.Password;
               
        }
 
    }
}
