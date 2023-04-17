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
    public class DetailsRepository : RepositoryBase // ,IDetailsRepo
    {
        public DetailsModel GetDetails(int ItemId)
        {
            //public ItemModel item { get; set; } // 该商品的基本信息
            //public UserModel seller { get; set; } //商家 
            //public List<UserModel> consumers { get; set; }//应该对应评论表-评论区消费者
            ItemRepository itemRepo = new ItemRepository();// 调用item方法
            UserRepository userRepo = new UserRepository();

            DetailsModel detailsModel = new DetailsModel();//返回值

            detailsModel.item = itemRepo.GetById(ItemId); // detailsModel.item.SellerName
            // 获取卖家信息
            detailsModel.seller = userRepo.GetByUsername(detailsModel.item.SellerName);
            detailsModel.comments = new List<ItemCommentModel>();
            // 根据商品id获取评论区评论和人员
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [ItemComment] where ItemId=@ItemId";
                command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item_tmp = new ItemCommentModel()
                        {
                            ItemId = (int)reader[0],
                            UserId = reader[1].ToString(),
                            Comment = reader[2].ToString(),
                            Username = reader[3].ToString(), 
                        };
                        detailsModel.comments.Add(item_tmp);
                    }
                }
            }
            return detailsModel;
        }// end public

        public void TmpSet(int ItemId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "truncate table TmpDetails;Insert Into TmpDetails Values(@ItemId)";
                command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) { }
                }
            }//end using
        }// end public
        public int TmpGet()
        {
            int ItemId = -1;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from TmpDetails";
                // command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) {
                        ItemId = (int)reader[0];
                    }
                }
            }//end using
            return ItemId;
        }// end public

        public void TmpCollageSet(int ItemId)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "truncate table TmpCollage;Insert Into TmpCollage Values(@ItemId)";
                command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) { }
                }
            }//end using
        }

        public int TmpCollageGet()
        {
            int ItemId = -1;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "Select * from TmpCollage";
                // command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ItemId = (int)reader[0];
                    }
                }
            }//end using
            return ItemId;
        }// end public

        // 拼单表
        public List<CollageModel> GetCollage()
        {
            List<CollageModel> item = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                item = new List<CollageModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from [ItemCollage] ";
               // command.Parameters.Add("@group", SqlDbType.NVarChar).Value = group;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item_tmp = new CollageModel()
                        {
                            ItemId = (int)reader[0],
                            UserId = reader[1].ToString(),
                            Username = reader[2].ToString(), 
                        };
                        item.Add(item_tmp);
                    }
                }
            }
            return item;
        }
        // 写入拼单表
        public void SetCollageById(int ItemId,string Username)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "Insert Into ItemCollage Values(@ItemId,(Select Id From [User] Where Username=@Username),@Username) ";
                command.Parameters.Add("@ItemId", SqlDbType.Int).Value = ItemId;
                command.Parameters.Add("@Username", SqlDbType.NChar).Value = Username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read()) { }
                }
            }//end using
        }
    }
}
