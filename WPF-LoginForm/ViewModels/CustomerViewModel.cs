using System;
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


namespace WPF_LoginForm.ViewModels
{
    public class CustomerViewModel : ViewModelBase // 用户
    {
        //Fields 字段
        private string _testusername;
        private IUserRepository userRepository;

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

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png;*.jpg; *.jpeg; *.gif; *.bmp)|*.png;*.jpg; *.jpeg; *.gif; *.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //string serverUrl = "http://yourserver.com/upload";
                //string serverUrl = "F:\\abc\\毕设\\项目\\Login-In-WPF-MVVM-C-Sharp-and-SQL-Server-main\\ligub2\\loogin2\\WPF-LoginForm\\Assets\\userHead\\";
                string serverUrl = "F:\\abc";
                string filePath = openFileDialog.FileName;

                WebClient client = new WebClient();

                client.UploadFile(serverUrl, filePath);

                MessageBox.Show("上传成功！");
            }
        }

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
