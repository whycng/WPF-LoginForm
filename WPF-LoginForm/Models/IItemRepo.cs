﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public interface IItemRepo
    {
        ItemModel GetById(int id);// 获取商品信息
        List<ItemModel> GetItemAll();
        List<ItemModel> GetBySellerName(string sellername); // 获取商品信息
        List<ItemModel> GetByClassify(string classify);// 获取商品信息
        void SetCart(int ItemId, string Username); // 塞进购物车数据库
        List<ItemModel> GetCart();// 获取购物车数据库
        void DelCartById(int Id);// 根据商品id删除购物车中某商品
        void EmptyCart();
        void AddCartById(int Id);
        void SubCartById(int Id);
        // void SetHisOrdById(int Id);// 根据商品id给历史订单数据
        void InsertItem( ItemModel Item, string photoName); // 插入商品

        void SetHisOrd(); // 直接把购物车塞进历史订单
        List<ItemModel> GetHisOrd();// 获取历史订单数据
        void DelHistById(int Id);//根据商品id删除历史订单中某商品
        List<ItemModel> GetHisOrdByVague(string vage);// 根据商品名获取
        bool IsMerchant(string username); // 判断为商家
        void DelItemById(int Id);
        void SetPriceById(int Id, string Price);
        void SetAmountById(int Id, int Amount);

        // 商品评论
        void SetCommentByUser(int ItemId, string Username, string comment);// 用户评论

        // 申诉单 GetAppealBySellerName
        List<AppealModel> GetAppealAll();
        List<AppealModel> GetAppealBySellerName(string sellername);
        void SetAppealByUsername(string username, int itemId ,string UsreReason);
        void UpdataAppealReplyByItemId(int itemId, string Reply);// 商家回复
        // 临时表 临时商家名
        void TmpSetSellername(string sellername);
        string TmpGetSellername();
        //购物车购买之后库存需要相应的减少
        void DelAmountByCart();
        // Seller
        SellerModel GetSellerModelBySellername(string sellername);
        void AddWatchBySellername(string sellername);
    }
}
 