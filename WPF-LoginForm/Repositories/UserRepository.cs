using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WPF_LoginForm.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
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
                            UserPhoto =  
                            new Uri("/assets/userHead/" + reader[6].ToString(), UriKind.RelativeOrAbsolute),
                            // sex.....
                            Sex = reader[7].ToString(),
                            Address = reader[8].ToString(),
                            Phone = reader[9].ToString(),   
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

        public void SetUserPhoto(string username, string filename)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET UserPhoto=@filename WHERE Username=@username;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@filename", SqlDbType.NVarChar).Value = filename;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                    else
                    {

                    }
                }
            }// end using
        }//end public

        public void SetNameByUserName(string username, string name)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET Name=@name WHERE Username=@username;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@name", SqlDbType.NVarChar).Value = name;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    } 
                }
            }// end using
        }//end public
        public void SetSexByUserName(string username, string sex)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET Sex=@sex WHERE Username=@username;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@sex", SqlDbType.NVarChar).Value = sex;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }//end public
        public void SetAddressByUserName(string username, string address)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET Address=@address WHERE Username=@username;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@address", SqlDbType.NVarChar).Value = address;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }//end public
        public void SetPhoneByUserName(string username, string phone)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET Phone=@phone WHERE Username=@username;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@phone", SqlDbType.NVarChar).Value = phone;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using

        }//end public
        public void SetEmailByUserName(string username, string email)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET Email=@email WHERE Username=@username;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }//end public

        public void SetPasswordByUserName(string username, string password)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "UPDATE [User] SET Password=@password WHERE Username=@username;";
                command.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = password;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }
        public List<UserModel> GetSeller()
        {
            List<UserModel> Seller = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                Seller = new List<UserModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE PCL=2;";
                //command.Parameters.Add("@sellername", SqlDbType.NVarChar).Value = sellername;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var t = new UserModel()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            Name = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString(),
                            UserPhoto =
                            new Uri("/assets/userHead/" + reader[6].ToString(), UriKind.RelativeOrAbsolute),
                            // sex.....
                            Sex = reader[7].ToString(),
                            Address = reader[8].ToString(),
                            Phone = reader[9].ToString(),
                        };
                        Seller.Add(t);
                    }
                }
            }
            return Seller;
        }// end public

        public List<UserModel> GetUser()
        {
            List<UserModel> User = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                User = new List<UserModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE PCL=1;";
                //command.Parameters.Add("@sellername", SqlDbType.NVarChar).Value = sellername;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var t = new UserModel()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            Name = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString(),
                            UserPhoto =
                            new Uri("/assets/userHead/" + reader[6].ToString(), UriKind.RelativeOrAbsolute),
                            // sex.....
                            Sex = reader[7].ToString(),
                            Address = reader[8].ToString(),
                            Phone = reader[9].ToString(),
                        };
                        User.Add(t);
                    }
                }
            }
            return User;
        }// end public

        public UserModel Search(UserModel user)
        {
            UserModel isSearch = null;
            if (user is null) 
                return null;
            user.Phone = user.Phone ?? "";
            user.Email = user.Email ?? "";
            user.Username = user.Username ?? "";
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] \r\nWHERE Phone LIKE @Phone \r\nOR email LIKE @Email \r\nOR username LIKE @Username;";
                command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = user.Phone;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = user.Email;
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = user.Username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // isSearch = true;

                        isSearch = new UserModel()
                        {
                            Id = reader[0].ToString(),
                            Username = reader[1].ToString(),
                            Password = reader[2].ToString(),
                            Name = reader[3].ToString(),
                            LastName = reader[4].ToString(),
                            Email = reader[5].ToString(),
                            UserPhoto =
    new Uri("/assets/userHead/" + reader[6].ToString(), UriKind.RelativeOrAbsolute),
                            
                            Sex = reader[7].ToString(),
                            Address = reader[8].ToString(),
                            Phone = reader[9].ToString(),
                        };
                    }
                }
            }// end using
            return isSearch;
        }

        public void SetAddFriend(UserModel ToUser, UserModel FromUser, string Reason)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Insert Into " +
                    "AddFriend(FromUsername,FromUserId,ToUsername,ToUserId,Reason)\r\n" +
                    "Select @FromUsername,t.Id,@ToUsername,q.Id,@Reason " +
                    "From [User] t,[User] q \r\n" +
                    "Where t.Username=@FromUsername And q.Username=@ToUsername";
                command.Parameters.Add("@FromUsername", SqlDbType.NVarChar).Value = FromUser.Username;
                command.Parameters.Add("@ToUsername", SqlDbType.NVarChar).Value = ToUser.Username;
                command.Parameters.Add("@Reason", SqlDbType.Text).Value = Reason;
                 using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
     
                    }
                }
            }// end using
        }// end public 

        public List<AddFriModel> GetAddFriByNowUsername(string NowUsername)
        {
            List<AddFriModel> AddFriList = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                AddFriList = new List<AddFriModel>();
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from AddFriend Where ToUsername=@NowUsername";
                command.Parameters.Add("@NowUsername", SqlDbType.NVarChar).Value = NowUsername;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var t = new AddFriModel()
                        {
                            FromUsername = reader[0].ToString(),
                            FromUserId = reader[1].ToString(),
                            ToUsername = reader[2].ToString(),
                            ToUserId = reader[3].ToString(),
                            Reason = reader[4].ToString(),
                            
                        };
                        AddFriList.Add(t);
                    }
                }
            }
            return AddFriList;
        }// end public
    
        public void AddFriend(string FromUsername, string ToUsername)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "Insert Into Friend(Id,Username,FId,FUsername)\r\n" +
                    "Select t.Id,@FromUsername,q.Id,@ToUsername \r\n" +
                    "From [User] t,[User] q \r\n" +
                    "Where t.Username=@FromUsername And q.Username=@ToUsername " +
                    "And Not Exists\r\n" +
                    "( Select * from Friend \r\n" +
                    "Where Username=@FromUsername And FUsername=@ToUsername);\r\n" +
                    "Insert Into Friend(Id,Username,FId,FUsername)\r\n" +
                    "Select t.Id,@ToUsername,q.Id,@FromUsername \r\n" +
                    "From [User] t,[User] q \r\n" +
                    "Where t.Username=@ToUsername And q.Username=@FromUsername " +
                     "And Not Exists\r\n" +
                    "( Select * from Friend \r\n" +
                    "Where Username=@ToUsername And FUsername=@FromUsername);";
                command.Parameters.Add("@FromUsername", SqlDbType.NVarChar).Value = FromUsername;
                command.Parameters.Add("@ToUsername", SqlDbType.NVarChar).Value = ToUsername;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {

                    }
                }
            }// end using
        }

        public bool IsAdmin(string Username)
        {
            bool IsAdmin = false;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {

                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [User] WHERE Username=@Username AND PCL='1';";
                command.Parameters.Add("@Username", SqlDbType.NVarChar).Value = Username;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        IsAdmin = true;
                    }
                }
            }
            return IsAdmin;
        }
    }
}
