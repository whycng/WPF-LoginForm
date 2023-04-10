using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models // 废弃
{
    public class CompBottonModel // 用于购物车 
    {
        public CompBottonModel() { }

        private string _id;
        public string Id { get { return _id; } set { _id = value; } }

        private Boolean _isCheck;

        public Boolean IsCheck { get { return _isCheck; } set { _isCheck = value; } }
    }
    
}
