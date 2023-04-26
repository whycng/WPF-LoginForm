using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace WPF_LoginForm.Repositories
{
    public class MessRepository: RepositoryBase,IMessRepository
    {
        // 应该拿到最后一个消息
        public MessModel LastMessageModel(string FromUserId, string ToUserId )
        {
            MessModel messModel = null;
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select M_ID,M_Message,M_Time from [Message] where M_FromUserID=@FromUserId AND M_ToUserID=@ToUserId ORDER BY M_ID DESC";
                command.Parameters.Add("@FromUserId", SqlDbType.NVarChar).Value = FromUserId;
                command.Parameters.Add("@ToUserId", SqlDbType.NVarChar).Value = ToUserId;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                         messModel = new MessModel()
                        {
                            M_ID = (int)reader[0],
                            M_Message = reader[1].ToString(),
                            M_Time = reader[2].ToString(),
                        }; 
                    }
                }
            } 

            return messModel;
        }

        public void SendMessage(string FromUsername, string ToUsername, string Text)
        {
            using (var connection = GetConnection())
            using (var command = new SqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = " \tInsert Into Message\r\n\t" +
                    "(M_Message,M_FromUsername,M_ToUsername,M_FromUserID,M_ToUserID) \r\n\t" +
                    "Values(@Text,@FromUsername,@ToUsername,\r\n\t" +
                    "(Select Id from [User] Where Username='admin'),\r\n\t" +
                    "(Select Id from [User] Where Username='test') \r\n\t) ;";
                command.Parameters.Add("@Text", SqlDbType.Text).Value = Text;
                command.Parameters.Add("@FromUsername", SqlDbType.NVarChar).Value = FromUsername;
                command.Parameters.Add("@ToUsername", SqlDbType.NVarChar).Value = ToUsername;
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                       
                    }
                }
            }// end using
        }// end public


    }
}
