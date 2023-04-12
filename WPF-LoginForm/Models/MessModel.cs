using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class MessModel
    {
        public int M_ID { get; set; }
        public string M_Message { get; set; }
        // public bit M_Status { get; set; }
        public string M_Time { get; set; }
        public string M_FromUserID { get; set; }
        public string M_FromUsername { get; set; }
        public string M_ToUserID { get; set; }
        public string M_ToUsername { get; set; } 
    }
}
