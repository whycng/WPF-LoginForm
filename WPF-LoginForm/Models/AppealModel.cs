using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class AppealModel
    {
        public int AppId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string SellerName { get; set; }
        public string UserName { get; set; }
        public string TimeApp { get; set; }
        public string UserReason { get; set; } 
        public string SellerReply { get; set; }
        public string Done { get; set; } 
    }
}
