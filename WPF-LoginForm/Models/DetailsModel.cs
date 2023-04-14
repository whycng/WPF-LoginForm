using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class DetailsModel
    {
        public ItemModel item {  get; set; } // 该商品的基本信息
        public UserModel seller { get; set; } //商家
        // 店铺页面
        //正在拼单
        public List<UserModel> consumers { get; set; }//应该对应评论表-评论区消费者
    }
}
