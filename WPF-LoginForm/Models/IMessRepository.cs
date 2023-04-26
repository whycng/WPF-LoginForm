using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public interface IMessRepository
    {
        MessModel LastMessageModel(string FromUserId, string ToUserId );
        // 发送信息
        void SendMessage(string FromUsername, string ToUsername, string Text);
    }
}
