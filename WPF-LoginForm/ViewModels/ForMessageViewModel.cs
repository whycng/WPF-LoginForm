
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPF_LoginForm.CustomControls;
using WPF_LoginForm.Models;
using WPF_LoginForm.Commands;
using GalaSoft.MvvmLight.Command;

namespace WPF_LoginForm.ViewModels
{
    public class ForMessageViewModel : ViewModelBase // 45.00
    {

        #region MainWindow For Message

        #region Properties
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        public string LastSeen { get; set; }
        // test
        public string defaultContactPhoto;
        public string DefaultContactPhoto { 
            get => defaultContactPhoto; set {
                defaultContactPhoto = "/assets/5.jpg";
            }
        }


        #endregion

        #endregion

        #region Status Thumbs
        #region Properties
        public ObservableCollection<StatusDataModel> statusThumbsCollection { get; set; }// = new ObservableCollection<StatusThumbs>();   
        #endregion
        #region Logics
        void LoadStatusThumbs()
        {
            statusThumbsCollection = new ObservableCollection<StatusDataModel>()
            {
                new StatusDataModel() {
                    IsMeAddStatus = true,
                },
                new StatusDataModel()
                {
                    ContactName="Mike",
                    ContactPhoto=new Uri("/assets/1.png", UriKind.RelativeOrAbsolute) ,
                    StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false,
                },
                new StatusDataModel()
                {
                    ContactName="Steve",
                    ContactPhoto=new Uri("/assets/2.jpg", UriKind.RelativeOrAbsolute) ,
                    StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false,
                },
                new StatusDataModel()
                {
                    ContactName="DaaVV",
                    ContactPhoto=new Uri("/assets/3.jpg", UriKind.RelativeOrAbsolute) ,
                    StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false,
                },
                new StatusDataModel()
                {
                    ContactName="Miles",
                    ContactPhoto=new Uri("/assets/4.jpg", UriKind.RelativeOrAbsolute) ,
                    StatusImage=new Uri("/assets/3.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false,
                }
            };
            OnPropertyChanged("statusThumbsCollection");

        }
        #endregion
        #endregion

        #region Chats List
        #region properties
        public ObservableCollection<ChatListData> Chats { get; set; }
        #endregion
        #region Logics
        void LoadChats()
        {
            Chats = new ObservableCollection<ChatListData>()
            {
                new ChatListData()
                {

                    ContactName = "Billy",
                    ContactPhoto = new Uri("/Assets/6.jpg",UriKind.RelativeOrAbsolute),
                    Message = "Hey,Whats up? 你好，你好，你end",
                    LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
                    ChatIsSelected=true,
                },
                new ChatListData()
                {
                    ContactName = "Mike",
                    ContactPhoto = new Uri("/Assets/6.jpg",UriKind.RelativeOrAbsolute),
                    Message = "小小英雄",
                    LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
                    ChatIsSelected=true,

                },
                new ChatListData()
                {
                    ContactName = "大卫在此",
                    ContactPhoto = new Uri("/Assets/6.jpg",UriKind.RelativeOrAbsolute),
                    Message = "在？---xx--x-x-x-x-x-x-xx-x-x-xx-xx-x-x-x-x-x-xx-x-",
                    LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
                    ChatIsSelected=true,

                },
                new ChatListData()
                {
                    ContactName = "Billy",
                    ContactPhoto = new Uri("/Assets/7.png",UriKind.RelativeOrAbsolute),
                    Message = "小明同学你好啊",
                    LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
                    ChatIsSelected=true,

                },
                new ChatListData()
                {
                    ContactName = "Billy",
                    ContactPhoto = new Uri("/Assets/8.jpg",UriKind.RelativeOrAbsolute),
                    Message = "Hey,Whats up? 你好，你好，你",
                    LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
                    ChatIsSelected=true,
                },
            };
            OnPropertyChanged(nameof(Chats)); // ??
        }
        #endregion

        #region Commands

        // to get the contactName of selected chat so that we can open corresponding conversation
        protected ICommand _getSelectedChatCommand; 

        public ICommand GetSelectedChatCommand => _getSelectedChatCommand ??= new Commands.RelayCommand(parameterX =>
        {   
            if(parameterX is ChatListData v)
            {
                // getting contactName from selected chat
                ContactName = v.ContactName;
                OnPropertyChanged("ContactName");

                // 
                ContactPhoto = v.ContactPhoto;
                OnPropertyChanged("ContactPhoto");
            }
            
        });


        #endregion

        #endregion

        #region Conversation
        protected ObservableCollection<ChatConversation> mConversations;
        public ObservableCollection<ChatConversation> Conversations
        {
            get => mConversations;
            set
            {
                mConversations = value;
                OnPropertyChanged(nameof(Conversations));
            }
        }
        #region Properties



        #endregion

        #region Logics

        void LoadChatConversation() { 
            
            if(connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            if(Conversations == null)
            {
                Conversations = new ObservableCollection<ChatConversation>();
            }
            using (SqlCommand com = new SqlCommand("select * from conversations where ContactName='Mike'",connection))
            {
                using(SqlDataReader reader = com.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // to set data format
                        // Like this   Jun 15, 01:15 PM = MMM dd, hh:mm tt
                        string MsgReceivedOn = !string.IsNullOrEmpty(reader["MsgReceivedOn"].ToString()) ?
                            Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";
                        string MsgSentOn = !string.IsNullOrEmpty(reader["MsgSentOn"].ToString()) ?
                            Convert.ToDateTime(reader["MsgSentOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";

                        var conversation = new ChatConversation() { 
                        
                            ContactName = reader["ContactName"].ToString(),
                            ReceivedMessage = reader["ReceivedMsgs"].ToString(),
                            MsgReceivedOn = MsgReceivedOn,// reader["MsgReceivedOn"].ToString(),
                            SentMessage = reader["SentMsgs"].ToString(),
                            MsgSentOn = MsgSentOn,// reader["MsgSentOn"].ToString(),
                            IsMessageReceived = string.IsNullOrEmpty(reader["ReceivedMsgs"].ToString()) ? false:true
                        };
                        Conversations.Add(conversation);
                        OnPropertyChanged(nameof(Conversation));
                    }
                }
                    
            }
            
        }
        #endregion


        #endregion

        SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\abc\毕设\项目\Login-In-WPF-MVVM-C-Sharp-and-SQL-Server-main\ligub2\loogin2\WPF-LoginForm\Database\Database1.mdf;Integrated Security=True");
        public ForMessageViewModel() 
        {
            LoadChats();
            LoadStatusThumbs();
            LoadChatConversation();
        }
        
    }
}
