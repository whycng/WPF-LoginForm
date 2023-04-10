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
        List<ItemModel> GetByClassify(string classify);
        void SetCart(int Id); // 塞进购物车数据库
        List<ItemModel> GetCart();// 获取购物车数据库
        void DelCartById(int Id);
    }
}
