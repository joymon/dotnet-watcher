using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JoymonsCode.DotNetWatcher.Model
{
    public class ExceptionNotification:NotificationBase
    {
        private string myVar;

        public string Messege
        {
            get { return myVar; }
            set
            {
                myVar = value;
                base.OnPropertyChanged<string>(() => Messege);
            }
        }
        
    }
}
