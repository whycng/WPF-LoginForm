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

namespace WPF_LoginForm.ViewModels
{
    public class CustomerViewModel : ViewModelBase // 用户
    {
        //Fields 字段
        private string _testusername;
        private IUserRepository userRepository;

        //Properties

        public string Testusername { get => _testusername;
            set {
                _testusername = value;
                OnPropertyChanged(nameof(Testusername));
            }//=> _testusername = value;
        }
        

        // -------------
        public CustomerViewModel() // 构造函数
        {
            userRepository = new UserRepository();

            // Testusername = "构造函数"; // 显示
            TestFun();
        }

        public void TestFun()
        {
            var test_user = userRepository.GetByUsername("admin");
            Testusername = test_user.Username;
        }

        public void Test_data()
        {

        }

    }
}
