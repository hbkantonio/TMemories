using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class Friend
    {
        public string UserId { get; set; }
        public string FriendUserId { get; set; }
        public int FriendStatusId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        public virtual FriendStatus FriendStatus { get; set; }
        public virtual User FriendUser { get; set; }
        public virtual User User { get; set; }
    }
}
