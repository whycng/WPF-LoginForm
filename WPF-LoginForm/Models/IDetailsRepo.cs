using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public interface IDetailsRepo // 废弃
    {
        DetailsModel GetDetails(int ItemId);
    }
}
