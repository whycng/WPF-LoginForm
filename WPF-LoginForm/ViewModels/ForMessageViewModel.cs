
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
using WPF_LoginForm.Repositories;
using System.Threading;

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
        private IUserRepository userRepository; 
        private IMessRepository messRepository;
        private UserAccountModel _currentUserAccount;
        private MessModel _messModel;
        public string DefaultContactPhoto { 
            get => defaultContactPhoto; set {
                defaultContactPhoto = "/assets/5.jpg";
            }
        }
        public UserAccountModel CurrentUserAccount
        {
            get
            {
                return _currentUserAccount;
            }

            set
            {
                _currentUserAccount = value;
                OnPropertyChanged(nameof(CurrentUserAccount));
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
            // 数据修改为来自数据库 获取当前用户的朋友-Friend Tbale -->获取 ContactName,ContactPhoto
            List<string> fri;// 
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            fri = userRepository.GetFriByUsername(user.Username);// 当前用户所有朋友Username
            statusThumbsCollection = new ObservableCollection<StatusDataModel>();// 该集合用于展示
            foreach ( var f in fri )
            {

                StatusDataModel t = new StatusDataModel()
                {
                    ContactName = userRepository.GetByUsername(f).Username,
                    ContactPhoto = userRepository.GetByUsername(f).UserPhoto, //new Uri(userRepository.GetByUsername(f).UserPhoto, UriKind.RelativeOrAbsolute),
                    // 应该是在线状态
                    StatusImage = new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus = false,
                };
                statusThumbsCollection.Add(t);
            }


            //statusThumbsCollection = new ObservableCollection<StatusDataModel>()
            //{
            //    new StatusDataModel() {
            //        IsMeAddStatus = true,
            //    },
            //    new StatusDataModel()
            //    {
            //        ContactName="Mike",
            //        ContactPhoto=new Uri("/assets/1.png", UriKind.RelativeOrAbsolute) ,
            //        StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
            //        IsMeAddStatus=false,
            //    },
            //    new StatusDataModel()
            //    {
            //        ContactName="Steve",
            //        ContactPhoto=new Uri("/assets/2.jpg", UriKind.RelativeOrAbsolute) ,
            //        StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
            //        IsMeAddStatus=false,
            //    },
            //    new StatusDataModel()
            //    {
            //        ContactName="DaaVV",
            //        ContactPhoto=new Uri("/assets/3.jpg", UriKind.RelativeOrAbsolute) ,
            //        StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
            //        IsMeAddStatus=false,
            //    },
            //    new StatusDataModel()
            //    {
            //        ContactName="Miles",
            //        ContactPhoto=new Uri("/assets/4.jpg", UriKind.RelativeOrAbsolute) ,
            //        StatusImage=new Uri("/assets/3.jpg", UriKind.RelativeOrAbsolute),
            //        IsMeAddStatus=false,
            //    }
            //};
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
            // 拿到当前用户和其正在聊天的人的对话的最后一句
            List<string> nameList = new List<string>();
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            nameList = userRepository.GetFriByUsername(user.Username);// 当前用户所有朋友Username
            Chats = new ObservableCollection<ChatListData>();// 该集合用于展示
            string fromId = userRepository.GetByUsername(user.Username).Id;
            foreach (var f in nameList)
            {
                string toId = userRepository.GetByUsername(f).Id;
                _messModel = messRepository.LastMessageModel(fromId, toId);
                if (_messModel != null) {
                    ChatListData t = new ChatListData()
                    {
                        // 取和朋友聊天的最后一句

                        ContactName = userRepository.GetByUsername(f).Username, 
                        ContactPhoto = userRepository.GetByUsername(f).UserPhoto,
                        Message = _messModel.M_Message,
                        LastMessageTime = _messModel.M_Time,
                        ChatIsSelected = true,

                    };
                    Chats.Add(t);
                } 
               
            }


            //Chats = new ObservableCollection<ChatListData>()
            //{
            //    new ChatListData()
            //    {

            //        ContactName = "Billy",
            //        ContactPhoto = new Uri("Assets/userHead/头像1.png                           ",UriKind.RelativeOrAbsolute),
            //        Message = "Hey,Whats up? 你好，你好，你end",
            //        LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
            //        ChatIsSelected=true,
            //    },
            //    new ChatListData()
            //    {
            //        ContactName = "Mike",
            //        ContactPhoto = new Uri("/Assets/6.jpg",UriKind.RelativeOrAbsolute),
            //        Message = "小小英雄",
            //        LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
            //        ChatIsSelected=true,

            //    },
            //    new ChatListData()
            //    {
            //        ContactName = "大卫在此",
            //        ContactPhoto = new Uri("/Assets/6.jpg",UriKind.RelativeOrAbsolute),
            //        Message = "在？---xx--x-x-x-x-x-x-xx-x-x-xx-xx-x-x-x-x-x-xx-x-",
            //        LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
            //        ChatIsSelected=true,

            //    },
            //    new ChatListData()
            //    {
            //        ContactName = "Billy",
            //        ContactPhoto = new Uri("/Assets/7.png",UriKind.RelativeOrAbsolute),
            //        Message = "小明同学你好啊",
            //        LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
            //        ChatIsSelected=true,

            //    },
            //    new ChatListData()
            //    {
            //        ContactName = "Billy",
            //        ContactPhoto = new Uri("/Assets/8.jpg",UriKind.RelativeOrAbsolute),
            //        Message = "Hey,Whats up? 你好，你好，你",
            //        LastMessageTime = "Tue, 12:58 PM 北京时间7点整",
            //        ChatIsSelected=true,
            //    },
            //};
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

        #region Properties
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

        protected string messageText;
        public string MessageText
        {
            get => messageText;
            set
            {
                messageText = value;
                OnPropertyChanged(nameof(MessageText));
            }
        }
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
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            messRepository = new MessRepository();
            _messModel = new MessModel();

            LoadChats();
            LoadStatusThumbs();
            LoadChatConversation();
        }
        
    }
}
