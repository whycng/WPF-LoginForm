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
        }
    }
}
