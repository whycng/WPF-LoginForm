using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public interface IItemRepo
    {
        List<ItemModel> GetById(int id);
        List<ItemModel> GetByGroup(string group); 
    }
}
