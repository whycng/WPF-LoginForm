
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
using System.Runtime.Remoting.Proxies;
using System.Data;
using System.Windows.Forms;
using System.Windows.Controls;
using WPF_LoginForm.Views;

namespace WPF_LoginForm.ViewModels
{
    public class ForMessageViewModel : ViewModelBase // 45.00
    {

        #region MainWindow For Message

        #region Properties 
        public MessModel messModel;
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LastSeen { get; set; }//
        //private string _fcontactName;//
                                     
        //public string FContactName
        //{
        //    get { return _fcontactName; }
        //    set { _fcontactName = "";
        //        OnPropertyChanged(nameof(FContactName)); }
        //}
        public string NowUsername { get; set; }// 
        public string FContactName { get; set; }// 
        // test
        // public string defaultContactPhoto;
        private IUserRepository userRepository; 
        private IMessRepository messRepository;
        private UserAccountModel _currentUserAccount;
        private MessModel _messModel;
        //public string DefaultContactPhoto {  
        //    get => defaultContactPhoto; set {
        //        defaultContactPhoto =  "/assets/5.jpg";
        //    }
        //}
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
            NowUsername = user.Username;
            fri = userRepository.GetFriByUsername(user.Username);// 当前用户所有朋友Username
            statusThumbsCollection = new ObservableCollection<StatusDataModel>();// 该集合用于展示
            foreach ( var f in fri )
            {

                StatusDataModel t = new StatusDataModel()
                {
                    ContactName = userRepository.GetByUsername(f).Username,
                    ContactPhoto = userRepository.GetByUsername(f).UserPhoto, //new Uri(userRepository.GetByUsername(f).UserPhoto, UriKind.RelativeOrAbsolute),
                    // 应该是在线状态
                    StatusImage = ContactPhoto,// new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
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

        void LoadContactInfo()
        {
            var user = userRepository.GetByUsername(Thread.CurrentPrincipal.Identity.Name);
            ContactPhoto = user.UserPhoto;
            ContactName = user.Name + "@" + user.Username;
            Email = user.Email;
            Phone = user.Phone;
        }

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
            if (Chats.Count > 0)
                FContactName = Chats[0].ContactName;
            else FContactName = "空";

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
        public GalaSoft.MvvmLight.Command.RelayCommand<string> _chageChatCommand;
        // to get the contactName of selected chat so that we can open corresponding conversation
        protected ICommand _getSelectedChatCommand;
        public GalaSoft.MvvmLight.Command.RelayCommand _addFriendCommand;
        // 发送信息 SendCommand
        public GalaSoft.MvvmLight.Command.RelayCommand _sendCommand;
        public GalaSoft.MvvmLight.Command.RelayCommand SendCommand
        {
            get
            {
                if (_sendCommand == null)
                    _sendCommand = new GalaSoft.MvvmLight.Command.RelayCommand(() => ExcuteSendCommand());
                return _sendCommand;
            }
        }
        //添加好友
        // 
        public GalaSoft.MvvmLight.Command.RelayCommand AddFriendCommand
        {
            get
            {
                if (_addFriendCommand == null)
                    _addFriendCommand = new GalaSoft.MvvmLight.Command.RelayCommand(( ) => ExcuteAddFriendCommand( ));
                return _addFriendCommand;
            }
        }
        public GalaSoft.MvvmLight.Command.RelayCommand<string> ChageChatCommand  
        {
            get
            {
                if (_chageChatCommand == null)
                    _chageChatCommand = new GalaSoft.MvvmLight.Command.RelayCommand<string>((parameter) => ExcuteChangeCommand(parameter));
                return _chageChatCommand;
            }
        }

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

        void ExcuteAddFriendCommand()
        {
            // 添加好友逻辑
            AddFriend view = new AddFriend();
            var r = view.ShowDialog();
            if (r.Value)
            {
                // 查询成功，添加信息正常发送

            }
            else
            {
                // 查询失败，不存在这个人
            }
        }
        void ExcuteChangeCommand(object parameter) // 点击左侧聊天用户切换聊天人
        {
            FContactName = parameter.ToString(); //聊天对象名
            Conversations = new ObservableCollection<ChatConversation>();
            LoadChatConversation();
            Conversations = new ObservableCollection<ChatConversation>(Conversations);
            // InvalidateVisual();
            // Refresh();
            // ItemsControl.UpdateLayout();
        }
        // test
        public GalaSoft.MvvmLight.Command.RelayCommand  _chageChatCommandX;
        // to get the contactName of selected chat so that we can open corresponding conversation
 
        public GalaSoft.MvvmLight.Command.RelayCommand  ChageChatCommandX
        {
            get
            {
                if (_chageChatCommandX == null)
                     _chageChatCommandX = new GalaSoft.MvvmLight.Command.RelayCommand(ExecuteTest1);

                return _chageChatCommandX;
            }
        }
        public GalaSoft.MvvmLight.Command.RelayCommand _chageChatCommandY;
        // to get the contactName of selected chat so that we can open corresponding conversation

        public GalaSoft.MvvmLight.Command.RelayCommand ChageChatCommandY
        {
            get
            {
                if (_chageChatCommandY == null)
                    _chageChatCommandY = new GalaSoft.MvvmLight.Command.RelayCommand(ExecuteTest2);

                return _chageChatCommandY;
            }
        }

        #endregion

        #endregion

        #region Conversation

        #region Properties
        protected ObservableCollection<ChatConversation> mConversations;
        public ObservableCollection<ChatConversation> Conversations
        {
            get { 
                return mConversations;
            }  
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
        void ExcuteSendCommand()
        {
            // 发送信息
            messRepository.SendMessage(NowUsername, FContactName, MessageText);
            LoadChatConversation();
        }
        void ExecuteTest1()
        {
            FContactName = "admin";//聊天对象名
            Conversations = new ObservableCollection<ChatConversation>();
            LoadChatConversation();
            Conversations = new ObservableCollection<ChatConversation>(Conversations);

        }
        void ExecuteTest2()
        {
            FContactName = "keni";//聊天对象名
            Conversations = new ObservableCollection<ChatConversation>();
            LoadChatConversation();
            Conversations = new ObservableCollection<ChatConversation>(Conversations);
            OnPropertyChanged(nameof(OnPropertyChanged));
        }
        void LoadChatConversation() { 
            
            if(connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            //if(Conversations == null)
            //{
            //    Conversations = new ObservableCollection<ChatConversation>();
            //}
            Conversations = new ObservableCollection<ChatConversation>();

            using (var command = new SqlCommand())
            {
                
               // connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from Message where M_ToUsername = @FContactName And M_FromUsername = @NowUsername";
                //command.Parameters.Add("@FContactName", SqlDbType.NVarChar).Value = FContactName;
                //command.Parameters.Add("@NowUsername", SqlDbType.NVarChar).Value = NowUsername;
                // 一个小问题，左右聊天，先这么解决
                command.Parameters.Add("@FContactName", SqlDbType.NVarChar).Value = NowUsername;
                command.Parameters.Add("@NowUsername", SqlDbType.NVarChar).Value = FContactName;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // to set data format
                        // Like this   Jun 15, 01:15 PM = MMM dd, hh:mm tt
                        //string MsgReceivedOn = !string.IsNullOrEmpty(reader["MsgReceivedOn"].ToString()) ?
                        //    Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";
                        string MsgSentOn = !string.IsNullOrEmpty(reader["M_Time"].ToString()) ?
                            Convert.ToDateTime(reader["M_Time"].ToString()).ToString("MMM dd, hh:mm tt") : "";
                        string MsgReceivedOn = !string.IsNullOrEmpty(reader["M_Time"].ToString()) ?
                          Convert.ToDateTime(reader["M_Time"].ToString()).ToString("MMM dd, hh:mm tt") : "";

                        var conversation = new ChatConversation()
                        {
                            Id = (int)reader["M_ID"],
                            ContactName = reader["M_ToUsername"].ToString(),
                            ReceivedMessage = reader["M_Message"].ToString(),
                            SentMessage = "".ToString(),


                            MsgReceivedOn = MsgReceivedOn,// reader["MsgReceivedOn"].ToString(),

                            MsgSentOn = MsgSentOn,// reader["MsgSentOn"].ToString(),
                            IsMessageReceived = true,//string.IsNullOrEmpty(SentMessage) ? false : true

                        };
                        Conversations.Add(conversation);
                        // OnPropertyChanged(nameof(Conversations));
                    }
                }
            }
            using (var command = new SqlCommand())
            {

              //  connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from Message where M_ToUsername = @NowUsername And M_FromUsername = @FcontactName";
                //command.Parameters.Add("@FcontactName", SqlDbType.NVarChar).Value = FContactName;
                //command.Parameters.Add("@NowUsername", SqlDbType.NVarChar).Value = NowUsername;

                command.Parameters.Add("@FcontactName", SqlDbType.NVarChar).Value = NowUsername;
                command.Parameters.Add("@NowUsername", SqlDbType.NVarChar).Value =  FContactName;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string MsgSentOn = !string.IsNullOrEmpty(reader["M_Time"].ToString()) ?
                          Convert.ToDateTime(reader["M_Time"].ToString()).ToString("MMM dd, hh:mm tt") : "";
                        string MsgReceivedOn = !string.IsNullOrEmpty(reader["M_Time"].ToString()) ?
                          Convert.ToDateTime(reader["M_Time"].ToString()).ToString("MMM dd, hh:mm tt") : "";
                        var M_ID = (int)reader["M_ID"];
                        var conversation = new ChatConversation()
                        {

                            ContactName = reader["M_ToUsername"].ToString(),
                            SentMessage = reader["M_Message"].ToString(),
                            ReceivedMessage = "".ToString(),


                            MsgReceivedOn = MsgReceivedOn,// reader["MsgReceivedOn"].ToString(),

                            MsgSentOn = MsgSentOn,// reader["MsgSentOn"].ToString(),
                            
                            IsMessageReceived = false,//string.IsNullOrEmpty(reader["SentMessage"].ToString()) ? false : true

                        };
                        int j;
                        for (j = 0; j < Conversations.Count; j++)
                        {
                            if (M_ID > Conversations[j].Id)
                            {
                                continue;
                            }
                            else
                            {
                                Conversations.Insert(j, conversation);
                                break;
                            }
                        }
                        if (j >= Conversations.Count)
                        {
                            Conversations.Add(conversation);
                        }

                        //if(M_ID > Conversations[i].Id)
                        //{
                        //    Conversations.Add(conversation);
                        //}
                        //else
                        //{
                        //    var con = Conversations[i];
                        //    Conversations[i] = conversation;
                        //    Conversations.Add(con);
                        //}
                        //i++;
                        // OnPropertyChanged(nameof(Conversations));
                    }
                }
            }

            #region 注释掉的SQL
            // 2 SQL
            //using (SqlCommand com = new SqlCommand("select * from Message where M_ToUsername=@FcontactName And M_FromUsername=@NowUsername", connection))
            //{
            //    using (SqlDataReader reader = com.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            // to set data format
            //            // Like this   Jun 15, 01:15 PM = MMM dd, hh:mm tt
            //            //string MsgReceivedOn = !string.IsNullOrEmpty(reader["MsgReceivedOn"].ToString()) ?
            //            //    Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";
            //            string MsgSentOn = !string.IsNullOrEmpty(reader["M_Time"].ToString()) ?
            //                Convert.ToDateTime(reader["M_Time"].ToString()).ToString("MMM dd, hh:mm tt") : "";
            //            string MsgReceivedOn = !string.IsNullOrEmpty(reader["M_Time"].ToString()) ?
            //              Convert.ToDateTime(reader["M_Time"].ToString()).ToString("MMM dd, hh:mm tt") : "";

            //            var conversation = new ChatConversation()
            //            {
            //                Id = (int)reader["M_ID"],
            //                ContactName = reader["M_ToUsername"].ToString(),
            //                ReceivedMessage = reader["M_Message"].ToString(),
            //                SentMessage = "".ToString(),


            //                MsgReceivedOn = MsgReceivedOn,// reader["MsgReceivedOn"].ToString(),

            //                MsgSentOn = MsgSentOn,// reader["MsgSentOn"].ToString(),
            //                IsMessageReceived = true,//string.IsNullOrEmpty(SentMessage) ? false : true

            //            };
            //            Conversations.Add(conversation);
            //            OnPropertyChanged(nameof(Conversations));
            //        }
            //    }

            //}// end using 
            //using (SqlCommand com = new SqlCommand("select * from Message where M_ToUsername='admin' And M_FromUsername='test'", connection))
            //{
            //    using (SqlDataReader reader = com.ExecuteReader())
            //    {
            //        // var i = 0;
            //        while (reader.Read())
            //        {
            //              string MsgSentOn = !string.IsNullOrEmpty(reader["M_Time"].ToString()) ?
            //                Convert.ToDateTime(reader["M_Time"].ToString()).ToString("MMM dd, hh:mm tt") : "";
            //            string MsgReceivedOn = !string.IsNullOrEmpty(reader["M_Time"].ToString()) ?
            //              Convert.ToDateTime(reader["M_Time"].ToString()).ToString("MMM dd, hh:mm tt") : "";
            //            var M_ID = (int)reader["M_ID"];
            //            var conversation = new ChatConversation()
            //            {

            //                ContactName = reader["M_ToUsername"].ToString(),
            //                SentMessage = reader["M_Message"].ToString(),
            //                ReceivedMessage  = "".ToString(),


            //                MsgReceivedOn = MsgReceivedOn,// reader["MsgReceivedOn"].ToString(),

            //                MsgSentOn = MsgSentOn,// reader["MsgSentOn"].ToString(),
            //                IsMessageReceived = false,//string.IsNullOrEmpty(reader["SentMessage"].ToString()) ? false : true

            //            };
            //            int j;
            //            for (j = 0; j < Conversations.Count; j++)
            //            {
            //                if (M_ID > Conversations[j].Id)
            //                {
            //                    continue;
            //                }
            //                else
            //                {
            //                    Conversations.Insert(j, conversation); 
            //                    break;
            //                }
            //            }
            //            if(j >= Conversations.Count)
            //            {
            //                Conversations.Add(conversation);
            //            }

            //            //if(M_ID > Conversations[i].Id)
            //            //{
            //            //    Conversations.Add(conversation);
            //            //}
            //            //else
            //            //{
            //            //    var con = Conversations[i];
            //            //    Conversations[i] = conversation;
            //            //    Conversations.Add(con);
            //            //}
            //            //i++;
            //            OnPropertyChanged(nameof(Conversations));
            //        }
            //    }

            //}// end using

            // 原 SQL
            //using (SqlCommand com = new SqlCommand("select * from conversations where ContactName='Mike'", connection))
            //{
            //    using (SqlDataReader reader = com.ExecuteReader())
            //    {
            //        while (reader.Read())
            //        {
            //            // to set data format
            //            // Like this   Jun 15, 01:15 PM = MMM dd, hh:mm tt
            //            string MsgReceivedOn = !string.IsNullOrEmpty(reader["MsgReceivedOn"].ToString()) ?
            //                Convert.ToDateTime(reader["MsgReceivedOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";
            //            string MsgSentOn = !string.IsNullOrEmpty(reader["MsgSentOn"].ToString()) ?
            //                Convert.ToDateTime(reader["MsgSentOn"].ToString()).ToString("MMM dd, hh:mm tt") : "";

            //            var conversation = new ChatConversation()
            //            {

            //                ContactName = reader["ContactName"].ToString(),
            //                ReceivedMessage = reader["ReceivedMsgs"].ToString(),
            //                MsgReceivedOn = MsgReceivedOn,// reader["MsgReceivedOn"].ToString(),
            //                SentMessage = reader["SentMsgs"].ToString(),
            //                MsgSentOn = MsgSentOn,// reader["MsgSentOn"].ToString(),
            //                IsMessageReceived = string.IsNullOrEmpty(reader["ReceivedMsgs"].ToString()) ? false : true
            //            };
            //            Conversations.Add(conversation);
            //            OnPropertyChanged(nameof(Conversation));
            //        }
            //    }

            //}// end using
            #endregion
        }
        #endregion


        #endregion
        SqlConnection connection = new SqlConnection("Server=(local); Database=MVVMLoginDb; Integrated Security=true");// "Server=(local); Database=MVVMLoginDb; Integrated Security=true";
        // SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\abc\毕设\项目\Login-In-WPF-MVVM-C-Sharp-and-SQL-Server-main\ligub2\loogin2\WPF-LoginForm\Database\Database1.mdf;Integrated Security=True");
        public ForMessageViewModel() 
        {
            userRepository = new UserRepository();
            CurrentUserAccount = new UserAccountModel();
            messRepository = new MessRepository();
            _messModel = new MessModel();
            Conversations = null;
            MessageText = string.Empty;

            LoadChats();
            LoadStatusThumbs();
            LoadChatConversation();
            LoadContactInfo();
        }
        
    }
}
