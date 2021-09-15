using JoymonsCode.DotNetWatcher.Core;
using JoymonsCode.DotNetWatcher.Model;
using System.Collections.ObjectModel;
namespace JoymonsCode.DotNetWatcher.ViewModel
{
    public class ResultViewModel :ViewModelBase
    {
        #region Properties

        private ObservableCollection<NotificationBase> myVar;

        public ObservableCollection<NotificationBase> Notifications
        {
            get { return myVar; }
            set
            {
                myVar = value;
                base.OnPropertyChanged<ObservableCollection<NotificationBase>>(() => Notifications);
            }
        }
        #endregion

        #region Constructor
        
        public ResultViewModel()
        {
            Notifications = new ObservableCollection<NotificationBase>();
        }
        #endregion

        #region Methods
        public void ReceiveNotification(NotificationBase notification)
        {
            Notifications.Add(notification);
        }
        #endregion
    }
}
