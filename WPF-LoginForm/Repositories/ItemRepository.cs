﻿using System;
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
        public List<ItemModel> GetById(int id)
        {
            throw new NotImplementedException();

        }

        // 根据商品卖家选择
        public List<ItemModel> GetByGroup(string group)
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
                            Id = reader[0].ToString(),
                            ItemName = reader[1].ToString(),
                            SellerName = reader[2].ToString(),
                            ItemShowText = reader[3].ToString(),
                            ItemPhoto = new Uri("/assets/item/" + reader[4].ToString() , UriKind.RelativeOrAbsolute),
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
                            Id = reader[0].ToString(),
                            ItemName = reader[1].ToString(),
                            SellerName = reader[2].ToString(),
                            ItemShowText = reader[3].ToString(),
                            ItemPhoto = new Uri("/assets/item/" + reader[4].ToString(), UriKind.RelativeOrAbsolute),
                        };
                        item.Add(item_tmp);
                    }
                }
            }
            return item;
        }

        public void SetCart(string id)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            { 
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
                            Id = reader[0].ToString(),
                            ItemName = reader[1].ToString(),
                            SellerName = reader[2].ToString(),
                            ItemShowText = reader[3].ToString(),
                            ItemPhoto = new Uri("/assets/item/" + reader[4].ToString(), UriKind.RelativeOrAbsolute),
                        };
                        item.Add(item_tmp);
                    }
                }
            }
        }
    }
}