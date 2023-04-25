using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class AddFriModel
    {
        public string FromUsername { get; set; }
        public string FromUserId { get; set; }
        public string ToUsername { get; set; }
        public string ToUserId { get; set; } 
        public string Reason { get; set; } 
    }
}
