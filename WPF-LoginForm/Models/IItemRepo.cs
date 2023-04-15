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
        ItemModel GetById(int id);
        List<ItemModel> GetBySellerName(string group); 
        List<ItemModel> GetByClassify(string classify);
        void SetCart(int Id); // 塞进购物车数据库
        List<ItemModel> GetCart();// 获取购物车数据库
        void DelCartById(int Id);// 根据商品id删除购物车中某商品

        // void SetHisOrdById(int Id);// 根据商品id给历史订单数据

        void SetHisOrd(); // 直接把购物车塞进历史订单
        List<ItemModel> GetHisOrd();// 获取历史订单数据
    }
}
 