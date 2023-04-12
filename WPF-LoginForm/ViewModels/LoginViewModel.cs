using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
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
            if (r.Value)
            {
                // 不需要，sql NEWID()生成
                //string user_id = GenerateCheckCodeNum(20);
                //model.Id = user_id;
                userRepository.Add(model);
                //var x = model.Username;
                //var y = model.Email;
            }
            else
                throw new NotImplementedException();
        }
    }
}
