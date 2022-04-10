using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class FriendStatus
    {
        public FriendStatus()
        {
            Friends = new HashSet<Friend>();
        }

        public int FriendStatusId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Friend> Friends { get; set; }
    }
}
