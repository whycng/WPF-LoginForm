using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_LoginForm.Models;
using WPF_LoginForm.Repositories;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Fields 字段
        private string _username;
        private SecureString _password;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;

        // private int rep = 0;//用于生成唯一用户id
   
        //Properties 属性：用于绑定 View 和 ViewModel
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                _username = value;
                // Username 或者_username
                OnPropertyChanged(nameof(Username)); // 通知properties改变，来自父类 Base 
            }
        }

        public SecureString Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }

            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //-> Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }
        public ICommand ShowPasswordCommand { get; }
        public ICommand RememberPasswordCommand { get; }

        //Constructor
        public LoginViewModel()
        { 
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            // 重载函数，因为不需要验证 ；恢复密码 
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassCommand("", ""));
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            bool validData;
            if (string.IsNullOrWhiteSpace(Username) || Username.Length < 3 ||
                Password == null || Password.Length < 3)
                validData = false;
            else
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(Username, Password));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(Username), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "* Invalid username or password";
            }
        }
        //private string GenerateCheckCodeNum(int codeCount) // 生成用户id
        //{
        //    string str = string.Empty;
        //    long num2 = DateTime.Now.Ticks + this.rep;
        //    this.rep++;
        //    Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
        //    for (int i = 0; i < codeCount; i++)
        //    {
        //        int num = random.Next();
        //        str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
        //    }
        //    return str;
        //}
        private void ExecuteRecoverPassCommand(string username, string email)
        {
            var model = new UserModel();

            AddUserView view = new AddUserView(model);
            var r = view.ShowDialog(); //返回值r就是 AddStuView页面【确定/取消】的结果
            if (r.Value) { // 点击了【确定】
                // 用于验证
                UserModel t = userRepository.GetByUsername(model.Username);

                // 如果用户名或者密码为空，报错
                if (model.Username == null || model.Username == string.Empty || model.Password == null || model.Password == string.Empty)
                {
                    System.Windows.MessageBox.Show("用户名或密码为空");
                }
                else if (t!=null) // 用户名没有被注册过
                {
                    System.Windows.MessageBox.Show("用户名已经存在");
                }
                else if (!Regex.IsMatch(model.Password, @"^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=_!])[a-zA-Z0-9@#$%^&+=_!]{8,}$")) // 判断密码强度
                { // 至少8-16个字符，至少1个大写字母，1个小写字母和1个数字，其他可以是任意字符：
                    System.Windows.MessageBox.Show("密码必须包含至少一个数字、一个小写字母、一个大写字母以及一个特殊字符(@#\r\n%^&+=_!] 长度至少为8位。");
                }
                else if (model.Email == null || model.Phone == null || model.Email == string.Empty || model.Phone == string.Empty)
                {
                    System.Windows.MessageBox.Show("手机号码或者邮箱必须填一项");
                }
                else if ( (!Regex.IsMatch(model.Phone, @"\d{3}-\d{8}|\d{4}-\{7,8}")) || (!Regex.IsMatch(model.Email, @"[\w!#$%&'*+/=?^_`{|}~-]+(?:\.[\w!#$%&'*+/=?^_`{|}~-]+)*@(?:[\w](?:[\w-]*[\w])?\.)+[\w](?:[\w-]*[\w])?")) )
                {// 手机号，邮箱校验
                    System.Windows.MessageBox.Show("手机号码或者邮箱格式不对");
                }
                else if (model.Address == null || model.Address == string.Empty)
                {
                    System.Windows.MessageBox.Show("地址没填");
                }
                else
                {
                    userRepository.Add(model);
                }
            }// end if 
        }// end private

        //int passJudge(string password)
        //{
        //    if( password.Length < 6 ) return 0;
        //    else
        //    {

        //    }
        //}
    }
}
