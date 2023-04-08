using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_LoginForm.Models
{
    public class StatusDataModel
    {
        public string ContactName { get; set; } 
        public Uri ContactPhoto { get; set; }   
        public Uri StatusImage { get; set; }  // StatusImage 
        public string StatusMessage  { get; set; }  
        public bool IsMeAddStatus { get; set; } 
    }
}
