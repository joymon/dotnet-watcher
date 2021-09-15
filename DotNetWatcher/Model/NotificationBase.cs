using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JoymonsCode.DotNetWatcher.Core;

namespace JoymonsCode.DotNetWatcher.Model
{
    public delegate void  NotificationCallback(NotificationBase notification);
    public abstract class NotificationBase:EntityBase
    {
    }
}
