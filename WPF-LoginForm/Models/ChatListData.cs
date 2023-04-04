using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class ChatListData
    {
        public string ContactName { get; set; }
        public Uri ContactPhoto{ get; set; }
        public string Message { get; set; }
        public string LastMessageTime { get; set; }
        public bool ChatIsSelected { get; set; }
    }
}
