using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public interface IUserRepository
    {
        bool AuthenticateUser(NetworkCredential credential);
        void Add(UserModel userModel);
        void Edit(UserModel userModel);
        void Remove(int id);
        UserModel GetById(int id);
        UserModel GetByUsername(string username);
        IEnumerable<UserModel> GetByAll();
        //...

        // 根据Username获取其朋友id列表/Username;
        List<string> GetFriByUsername(string username);
        // 根据用户名修改其UserPhoto
        void SetUserPhoto(string username, string filename);
        void SetNameByUserName(string username, string name);
        void SetSexByUserName(string username, string sex);
        void SetAddressByUserName(string username, string address);
        void SetPhoneByUserName(string username, string phone);
        void SetEmailByUserName(string username, string email);
        List<UserModel> GetSeller();// 拿到卖家  
        List<UserModel> GetUser();// 拿到普通用户 
    }
}
