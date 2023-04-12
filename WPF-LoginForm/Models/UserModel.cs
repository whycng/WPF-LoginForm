using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserPhoto { get; set; } // 用户头像
        public string Sex { get; set; }//性别
        public string Address { get; set; } //收货地址
        public string Phone { get; set; }//电话号码
    }
}
