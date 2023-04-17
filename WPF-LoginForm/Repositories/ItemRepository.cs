using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows;

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

        public void SetCart(int ItemId,string Username) //根据商品id将商品信息插入购物车
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            { 
                connection.Open();
                command.Connection = connection;
                //command.CommandText = "Insert Into ItemCart(Id,ItemName,price) Select " +
                //    "Id,ItemName,price From Item Where Id=@Id " +
                //    "And Not Exists(Select Id From ItemCart where Id=@Id) ";
                command.CommandText = " INSERT INTO ItemCart(Id,ItemName,price,UserId,Username,Amount) " +
                    "SELECT Id, ItemName, price, (SELECT Id FROM [User] WHERE Username=@Username), @Username,1 " +
                    "FROM Item WHERE Id=@ItemId AND NOT EXISTS (SELECT Id FROM ItemCart " +
                    "WHERE Id=@ItemId AND UserId=(SELECT Id FROM [User] WHERE Username=@Username)) ";
                command.Parameters.Add("@ItemId", SqlDbType.NVarChar).Value = ItemId;
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username;
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
            }//end using
        }

        public void AddCartById(int Id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Update ItemCart Set Amount = Amount+1   Where Id=@Id ";
                command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                using (var reader = command.ExecuteReader())
                {
                }
            }//end using
        }
        public void SubCartById(int Id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE ItemCart\r\nSET Amount = CASE WHEN Amount > 0 THEN Amount - 1 ELSE 0 END\r\nWHERE Id = @Id\r\n";
                command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                using (var reader = command.ExecuteReader())
                {
                }
            }//end using
        }

        public void SetHisOrd()// 直接把购物车塞进历史订单
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // 向HistoricalOrders表中插入值，值来自item，约束条件：Item.Id in ItemCart.Id AND Item.Id not in HistoricalOrders.Id
                //command.CommandText = "Insert Into HistoricalOrders(Id,ItemName,SellerName,ItemShowText,ItemPhoto,ItemClassify,reco,price,Amount) Select " +
                //    "Id,ItemName,SellerName,ItemShowText,ItemPhoto,ItemClassify,reco,price,(select Amount From ItemCart Where ItemCart.Id=Id) From Item Where Item.Id in (Select Id From ItemCart) And Item.Id Not In (Select Id From HistoricalOrders)";
                command.CommandText = "INSERT INTO HistoricalOrders\r\n(Id, ItemName, SellerName, ItemShowText, ItemPhoto, ItemClassify, reco, price, Amount)\r\nSELECT i.Id, i.ItemName, i.SellerName, i.ItemShowText, i.ItemPhoto, i.ItemClassify, i.reco, i.price, c.Amount\r\nFROM Item i\r\nJOIN ItemCart c ON i.Id = c.Id\r\nLEFT JOIN HistoricalOrders h ON i.Id = h.Id\r\nWHERE h.Id IS NULL;";

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
                            Amount = (int)reader[8],
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

        public void EmptyCart()
        {
             
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Delete From ItemCart ";
                using (var reader = command.ExecuteReader())
                {
                }
            }//end using
        }
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
                            Amount = (int)reader[5],
                        };
                        item.Add(item_tmp);
                    }
                }
            }// end using
            return item;
        }

        public void DelHistById(int Id) { //根据商品id删除历史订单中某商品
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Delete From HistoricalOrders Where Id = @Id";
                command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                using (var reader = command.ExecuteReader())
                {
                }
            }//end using
        }// end public
        public List<ItemModel> GetHisOrdByVague(string vage)
        {

            List<ItemModel> item = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                item = new List<ItemModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM HistoricalOrders WHERE CONCAT_WS(',', Id, ItemName, SellerName, ItemShowText, ItemClassify, price) LIKE '%' + @vage + '%';";
                command.Parameters.Add("@vage", SqlDbType.NVarChar).Value = vage; 
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
                            Amount = (int)reader[8],
                        };
                        item.Add(item_tmp);
                    }
                }
            }
            return item;
        }
    }
}
