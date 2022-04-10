using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int NotificationTypeId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUserId { get; set; }

        public virtual User CreationUser { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public virtual User User { get; set; }
    }
}
