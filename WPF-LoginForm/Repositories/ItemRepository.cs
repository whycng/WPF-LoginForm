using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;

namespace WPF_LoginForm.Repositories
{
    public class ItemRepository: RepositoryBase, IItemRepo 
    {
        public ItemModel GetById(int Id)
        {
            ItemModel item = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                 
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [Item] where Id=@Id";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = Id; // 一个bug ：SqlDbType.Int
                using (var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        item = new ItemModel()
                        {
                            Id = (int)reader[0],
                            ItemName = reader[1].ToString(),
                            SellerName = reader[2].ToString(),
                            ItemShowText = reader[3].ToString(),
                            ItemPhoto = new Uri("/assets/item/" + reader[4].ToString(), UriKind.RelativeOrAbsolute),
                            ItemClassify = reader[5].ToString(),
                            reco = reader[6].ToString(),
                            price = reader[7].ToString(),
                        };
                    }
                    else
                    {
                        throw new Exception("WR");
                    }
                    //var x1 = (int)reader[0];
                    //item = new ItemModel()
                    //{
                    //    Id = (int)reader[0],
                    //    ItemName = reader[1].ToString(),
                    //    SellerName = reader[2].ToString(),
                    //    ItemShowText = reader[3].ToString(),
                    //    ItemPhoto = new Uri("/assets/item/" + reader[4].ToString(), UriKind.RelativeOrAbsolute),
                    //    ItemClassify = reader[5].ToString(),
                    //    reco = reader[6].ToString(),
                    //    price = reader[7].ToString(),
                    //};
                }
            }
            return item;
        }



        // 根据商品卖家选择  
        public List<ItemModel> GetBySellerName(string group)
        {
            List<ItemModel> item = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                item = new List<ItemModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [Item] where SellerName=@group";
                command.Parameters.Add("@group", SqlDbType.NVarChar).Value = group;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item_tmp = new ItemModel()
                        {
                            Id = (int)reader[0] ,
                            ItemName = reader[1].ToString(),
                            SellerName = reader[2].ToString(),
                            ItemShowText = reader[3].ToString(),
                            ItemPhoto = new Uri("/assets/item/" + reader[4].ToString() , UriKind.RelativeOrAbsolute),
                            ItemClassify = reader[5].ToString(),
                            reco = reader[6].ToString(),   
                            price = reader[7].ToString(),
                        };
                        item.Add(item_tmp);
                    }
                }
            }
            return item;
        }

        // 根据商品分类选择
        public List<ItemModel> GetByClassify(string classify)
        {
            List<ItemModel> item = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                item = new List<ItemModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [Item] where ItemClassify=@classify";
                command.Parameters.Add("@classify", SqlDbType.NVarChar).Value = classify;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item_tmp = new ItemModel()
                        {
                            Id = (int)reader[0],
                            ItemName = reader[1].ToString(),
                            SellerName = reader[2].ToString(),
                            ItemShowText = reader[3].ToString(),
                            ItemPhoto = new Uri("/assets/item/" + reader[4].ToString(), UriKind.RelativeOrAbsolute),
                            ItemClassify = reader[5].ToString(),
                            reco = reader[6].ToString(),
                            price = reader[7].ToString(),
                        };
                        item.Add(item_tmp);
                    }
                }
            }
            return item;
        }

        public void SetCart(int Id) //根据商品id将商品信息插入购物车
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            { 
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Insert Into ItemCart(Id,ItemName,price) Select " +
                    "Id,ItemName,price From Item Where Id=@Id " +
                    "And Not Exists(Select Id From ItemCart where Id=@Id) ";
                command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                using (var reader = command.ExecuteReader())
                { 
                }
            }
        }
        public void DelCartById(int Id) // 根据商品id删除购物车中某商品
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Delete From ItemCart Where Id = @Id";
                command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                using (var reader = command.ExecuteReader())
                {
                }
            }
        }
        
        public void SetHisOrd()// 直接把购物车塞进历史订单
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // 向HistoricalOrders表中插入值，值来自item，约束条件：Item.Id in ItemCart.Id AND Item.Id not in HistoricalOrders.Id
                command.CommandText = "Insert Into HistoricalOrders(Id,ItemName,SellerName,ItemShowText,ItemPhoto,ItemClassify,reco,price) Select " +
                    "Id,ItemName,SellerName,ItemShowText,ItemPhoto,ItemClassify,reco,price From Item Where Item.Id in (Select Id From ItemCart) And Item.Id Not In (Select Id From HistoricalOrders)";
                   
                // command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                using (var reader = command.ExecuteReader())
                {
                }
            }
        }

        public List<ItemModel> GetHisOrd()
        {
            List<ItemModel> item = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                item = new List<ItemModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [HistoricalOrders]";
                // command.Parameters.Add("@classify", SqlDbType.NVarChar).Value = classify;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item_tmp = new ItemModel()
                        {
                            Id = (int)reader[0],
                            ItemName = reader[1].ToString(),
                            SellerName = reader[2].ToString(),
                            ItemShowText = reader[3].ToString(),
                            ItemPhoto = new Uri("/assets/item/" + reader[4].ToString(), UriKind.RelativeOrAbsolute),
                            ItemClassify = reader[5].ToString(),
                            reco = reader[6].ToString(),
                            price = reader[7].ToString(),
                        };
                        item.Add(item_tmp);
                    }
                }
            }
            return item;
        }

        //public void SetHisOrdById(int Id)// 根据商品id给历史订单数据
        //{
        //    using (var connection = GetConnection())
        //    using (var command = new SqlCommand())
        //    {
        //        connection.Open();
        //        command.Connection = connection;
        //        command.CommandText = "Insert Into HistoricalOrders(Id,ItemName,SellerName,ItemShowText,ItemPhoto,ItemClassify,reco,price) Select " +
        //            "Id,ItemName,SellerName,ItemShowText,ItemPhoto,ItemClassify,reco,price From Item Where Id=@Id " +
        //            "And Not Exists(Select Id From Item where Id=@Id) ";
        //        command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
        //        using (var reader = command.ExecuteReader())
        //        {
        //        }
        //    }
        //}


        public List<ItemModel> GetCart() // 获取购物车数据
        {
            List<ItemModel> item = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                item = new List<ItemModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [ItemCart]";
                // command.Parameters.Add("@classify", SqlDbType.NVarChar).Value = classify;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item_tmp = new ItemModel()
                        {
                            Id = (int)reader[0],
                            ItemName = reader[1].ToString(), 
                            price = reader[2].ToString(),
                        };
                        item.Add(item_tmp);
                    }
                }
            }
            return item;
        }
    }
}
