using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string SellerName { get; set; }
        public string ItemShowText { get; set; }
        public Uri ItemPhoto { get; set; } // 图片地址
        public string ItemClassify{ get; set; }
        public string reco { get; set; }
        public string price { get; set;}
 

    }
}
