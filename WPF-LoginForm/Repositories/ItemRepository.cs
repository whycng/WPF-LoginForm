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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Xml.Linq;
using System.Windows.Forms;

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
                            Amount = (int)reader[8],
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

        public void InsertItem(ItemModel Item, string photoName)
        { 
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            { 
                connection.Open();
                command.Connection = connection;
                command.CommandText = " INSERT INTO Item (Id, ItemName, SellerName, ItemShowText, ItemPhoto, ItemClassify, price, Amount)\r\nSELECT COUNT(*) + 1, @ItemName, @SellerName, @ItemShowText, @ItemPhoto, @ItemClassify, @price, @Amount FROM Item;\r\n";
               // command.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                command.Parameters.Add("@ItemName", SqlDbType.NVarChar).Value = Item.ItemName;
                command.Parameters.Add("@SellerName", SqlDbType.NVarChar).Value = Item.SellerName;
                command.Parameters.Add("@ItemShowText", SqlDbType.NVarChar).Value = Item.ItemShowText;
                command.Parameters.Add("@ItemPhoto", SqlDbType.NVarChar).Value = photoName;//Item.ItemPhoto;
                command.Parameters.Add("@ItemClassify", SqlDbType.NVarChar).Value = Item.ItemClassify;
                command.Parameters.Add("@price", SqlDbType.NVarChar).Value = Item.price;
                command.Parameters.Add("@Amount", SqlDbType.Int).Value = Item.Amount; 
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }// end public

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
                            Amount = (int)reader[8],
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
                            Amount = (int)reader[8],
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

                // 第二句主要更新Seller表
                /*
            因为ItemCart中一个Username对应了多个购物车项，而且每个购物车项
                可能属于不同的卖家。因此，我们需要使用GROUP BY子句来将相同的
                Username组合在一起，并计算它们所关联的所有购物车项的总金额。
                以下是一个可以实现您所需功能的SQL语句：

```
UPDATE Seller
SET TotalOrder = TotalOrder + c.Amount
FROM Seller s
JOIN (
    SELECT i.Sellername, SUM(c.Amount) AS Amount
    FROM ItemCart c
    JOIN Item i ON c.Id = i.Id
    GROUP BY i.Sellername
) c ON s.Username = c.Sellername;
```

这个SQL语句中，内部的SELECT语句使用JOIN操作将购物车项和商品关联在一起，
                并计算每个卖家的总销售额。然后，通过JOIN操作将这些数据
                与Seller表进行关联，并更新TotalOrder属性。


                第三行：总收入


                 */
                command.CommandText = "INSERT INTO HistoricalOrders\r\n" +
                    "(Id, ItemName, SellerName, ItemShowText, ItemPhoto, ItemClassify, reco, price, Amount)\r\n" +
                    "SELECT i.Id, i.ItemName, i.SellerName, i.ItemShowText, i.ItemPhoto, i.ItemClassify, i.reco, i.price, c.Amount\r\nFROM Item i\r\n" +
                    "JOIN ItemCart c ON i.Id = c.Id ;"
                    + " \r\nUPDATE Seller\r\nSET TotalOrder = TotalOrder + c.Amount\r\nFROM Seller s\r\nJOIN (\r\n    SELECT i.Sellername, SUM(CAST(c.Amount AS int)) AS Amount\r\n    FROM ItemCart c\r\n    JOIN Item i ON c.Id = i.Id\r\n    GROUP BY i.Sellername\r\n) c ON s.Sellername = c.SellerName;\r\n\r\nCREATE TABLE #SellerRevenue (\r\n    Sellername nvarchar(50),\r\n    ALLPrice decimal(10, 2)\r\n);\r\n\r\nINSERT INTO #SellerRevenue (Sellername, ALLPrice)\r\nSELECT ic.Username, SUM(CAST(ic.price AS double precision) * CAST(ic.Amount AS int)) AS ALLPrice\r\nFROM ItemCart ic\r\nGROUP BY ic.Username;\r\n\r\nUPDATE s\r\nSET Revenue = s.Revenue + sr.ALLPrice\r\nFROM Seller s\r\nINNER JOIN #SellerRevenue sr ON s.Sellername = sr.Sellername;\r\n\r\nDROP TABLE #SellerRevenue;"
                    + "";

                // command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                using (var reader = command.ExecuteReader())
                {
                }
            }
        }
        public List<ItemModel> GetItemAll()
        {
            List<ItemModel> item = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                item = new List<ItemModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [Item]";
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
                            Date = reader[9].ToString(),
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
                            Date = reader[9].ToString(),    
                        };
                        item.Add(item_tmp);
                    }
                }
            }
            return item;
        }

        public bool IsMerchant(string username)
        {
            bool isMerchant = false;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Item WHERE SellerName=@username;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {
                        isMerchant = true; 
                    } 
                }
            }
            return isMerchant;
        }// end public 

        public void DelItemById(int Id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "DELETE FROM Item WHERE Id = @Id;\r\n;";
                command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                         
                    }
                }
            }// end using
        }// end public
         
        public void SetPriceById(int Id, string Price)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE Item SET price = @Price WHERE id = @Id;\r\n";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                command.Parameters.Add("@Price", SqlDbType.NVarChar).Value = Price;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }

        public void SetAmountById(int Id, int Amount)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE Item SET Amount = @Amount WHERE id = @Id;\r\n";
                command.Parameters.Add("@Id", SqlDbType.Int).Value = Id;
                command.Parameters.Add("@Amount", SqlDbType.Int).Value = Amount;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }
        // 申诉单
        public List<AppealModel> GetAppealBySellerName(string sellername)
        {
            List<AppealModel> appeals = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                appeals = new List<AppealModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Appeal WHERE SellerName=@sellername;";
                command.Parameters.Add("@sellername", SqlDbType.NVarChar).Value = sellername;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item_tmp = new AppealModel()
                        {
                            AppId = (int)reader[0],
                            ItemId = (int)reader[1],
                            ItemName = reader[2].ToString(),
                            SellerName = reader[3].ToString(),
                            UserName = reader[4].ToString(),
                            TimeApp = reader[5].ToString(),
                            UserReason = reader[6].ToString(),
                            SellerReply = reader[7].ToString(),
                            Done = reader[8].ToString(),
                        };
                        appeals.Add(item_tmp);
                    }
                }
            }
            return appeals;
        }

        public void SetAppealByUsername(string username, int itemId, string UsreReason)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            { 
                connection.Open();
                command.Connection = connection;
                command.CommandText = "INSERT INTO Appeal " +
                 "(ItemId,ItemName,SellerName,UserName,UserReason,SellerReply,Done)" +
                 "SELECT @itemId, ItemName, SellerName, @username, @UsreReason,  '未回复',  '未完成'" +
                 "FROM  Item " +
                 "WHERE     Id = @itemId;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@itemId", SqlDbType.Int).Value = itemId;
                command.Parameters.Add("@UsreReason", SqlDbType.NVarChar).Value = UsreReason;
                using (var reader = command.ExecuteReader())
                {
                    if(reader.Read())
                    {

                    } 
                }
            }
        } // end public

        public void UpdataAppealReplyByItemId(int itemId, string Reply)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE Appeal Set SellerReply=@Reply Where ItemId=@itemId;"; 
                command.Parameters.Add("@Reply", SqlDbType.NVarChar).Value = Reply;
                command.Parameters.Add("@itemId", SqlDbType.Int).Value = itemId; 
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }
        }// end public
        public List<AppealModel> GetAppealAll()
        {
            List<AppealModel> appeals = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                appeals = new List<AppealModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM Appeal";
                // command.Parameters.Add("@sellername", SqlDbType.NVarChar).Value = sellername;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item_tmp = new AppealModel()
                        {
                            AppId = (int)reader[0],
                            ItemId = (int)reader[1],
                            ItemName = reader[2].ToString(),
                            SellerName = reader[3].ToString(),
                            UserName = reader[4].ToString(),
                            TimeApp = reader[5].ToString(),
                            UserReason = reader[6].ToString(),
                            SellerReply = reader[7].ToString(),
                            Done = reader[8].ToString(),
                        };
                        appeals.Add(item_tmp);
                    }
                }
            }
            return appeals;
        }// end publci 

        public void SetCommentByUser(int ItemId, string Username, string comment)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Insert INTO ItemComment(ItemId,UserId,Comment) Select @ItemId,Id,@comment FROM" +
                    " [User] Where Username=@Username;  ";
                command.Parameters.Add("@comment", SqlDbType.NVarChar).Value = comment;
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username;
                command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }
        }// end public

        public void TmpSetSellername(string sellername)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "truncate table TmpSellername;" +
                    "Insert Into TmpSellername(Sellername) Values (@sellername); ";
                command.Parameters.Add("@sellername", SqlDbType.NVarChar).Value = sellername; 
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }// end public

        public string TmpGetSellername()
        {
            string Sellername= string.Empty;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = " select * from TmpSellername;\r\n" +
                    "truncate table TmpSellername;";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Sellername = reader[0].ToString();
                    }
                }
            }// end using
            return Sellername;
        }

        public void DelAmountByCart()
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "  UPDATE t SET Amount = t.Amount - m.Amount FROM Item t JOIN ItemCart m ON m.Id = t.Id";
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                         
                    }
                }
            }// end using
        }

        public SellerModel GetSellerModelBySellername(string sellername)
        {
            SellerModel model = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                // 第一行刷新Appeal数据，第二行拿四个数据
                command.CommandText = "UPDATE Seller\r\nSET Appeal = t.AppealCount\r\nFROM (\r\n    SELECT Username, COUNT(*) AS AppealCount\r\n    FROM Appeal Where Done <> '完成'\r\n    GROUP BY Username\r\n) t\r\nWHERE Seller.Sellername = t.Username;"
                    + "Select * From Seller Where Sellername = @sellername";  
                command.Parameters.Add("@sellername", SqlDbType.NVarChar).Value = sellername;
                using (var reader = command.ExecuteReader())
                {
                   
                        //string x = reader[0].ToString();
                        //string x1 = reader[1].ToString();
                        //string x2 = reader[2].ToString();
                        //string x3 = reader[3].ToString();


                    if (reader.Read())
                    {
                        //int l = (int)reader[0];
                        //int x = (int)reader[1];
                        //double c = (double)reader[2];
                        //int n = (int)reader[3];
                        model = new SellerModel()
                        { // 第一个数据 Sellername 不需要
                            TotalOrder = (int)reader[1],
                            Watch = (int)reader[2],
                            Revenue = (double)reader[3],
                            Appeal = (int)reader[4],
                        }; 
                    }
                }
            }// end using
            return model;
        }// end public

        public void AddWatchBySellername(string sellername)
        { 
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE Seller\r\nSET Watch = Watch + 1\r\nWhere Sellername = @sellername";
                command.Parameters.Add("@sellername", SqlDbType.NVarChar).Value = sellername;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
               
                    }
                }
            }// end using 
        }

    }
}
