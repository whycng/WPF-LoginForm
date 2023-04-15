using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class ItemCommentModel
    {
        public int ItemId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public string Username { get; set; }
    }
}
