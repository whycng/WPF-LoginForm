﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WPF_LoginForm.Repositories
{
    public class UserRepository : RepositoryBase, IUserRepository // 继承
    {
        public void Add(UserModel userModel)
        {
            // var Id = userModel.Id;
            var Username = userModel.Username;
            var Password = userModel.Password;
            var Name = userModel.Name;
            var LastName = userModel.LastName;
            var Email = userModel.Email;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Insert Into [User](Id,Username,Password,Name,LastName,Email) " +
                    "Values( NEWID(),@Username,@Password,@Name,@LastName,@Email)";
                // command.Parameters.Add("@Id", SqlDbType.NVarChar).Value = Id;
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username;
                command.Parameters.Add("@Password", SqlDbType.NVarChar).Value = Password;
                command.Parameters.Add("@Name", SqlDbType.NVarChar).Value = Name;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email; 
                using (var reader = command.ExecuteReader())
                {

                }
            }
            
        }

        public bool AuthenticateUser(NetworkCredential credential)
        {
            bool validUser;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select *from [User] where username=@username and [password]=@password";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = credential.UserName;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = credential.Password;
                validUser = command.ExecuteScalar() == null ? false : true;
            }
            return validUser;
        }

        public void Edit(UserModel userModel)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<UserModel> GetByAll()
        {
            throw new NotImplementedException();
        }
        public UserModel GetById(int id)
        {
            throw new NotImplementedException();
        }
        public UserModel GetByUsername(string username)
        {
            UserModel user = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select *from [User] where username=@username";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UserModel()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = string.Empty,
                            Name = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString(),
                            UserPhoto = reader[6].ToString(), 
                            // sex.....
                        };
                    }
                }
            }
            return user;
        }
        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        // 根据Username获取其朋友id列表/Username;
        public List<string> GetFriByUsername(string Username)
        {
            List<string> fri_username_list = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                fri_username_list = new List<string>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select FUsername from [Friend] where Username=@Username";
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var fri_username = reader[0].ToString();

                        fri_username_list.Add(fri_username);
                    }
                }
            }
            return fri_username_list;
        }
    }
}
