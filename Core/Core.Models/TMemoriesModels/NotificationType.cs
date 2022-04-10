using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            Notifications = new HashSet<Notification>();
        }

        public int NotificationTypeId { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
