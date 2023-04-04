using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_LoginForm.ViewModels;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WPF_LoginForm.Models
{
    public class Sutdent:ViewModelBase
    {
        private int id;
        private string name;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged(nameof(Id));
                // Rais
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
                // Rais
            }
        }
    }
}
