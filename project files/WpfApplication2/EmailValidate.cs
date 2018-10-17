using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace StatsBR
{
    public class EmailValidate : ViewModelBase
    {
        private string emailAddress;
        [RegularExpression(@"\b[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b")]
        public string EmailAddress
        {
            get { return emailAddress; }
            set
            {
                emailAddress = value;
                this.OnPropertyChanged("EmailAddress");
            }
        }

    }
}
