using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class User
    {
        public User()
        {
            FriendFriendUsers = new HashSet<Friend>();
            FriendUsers = new HashSet<Friend>();
            NotificationCreationUsers = new HashSet<Notification>();
            NotificationUsers = new HashSet<Notification>();
            PostUsers = new HashSet<PostUser>();
            Posts = new HashSet<Post>();
            TrackMemories = new HashSet<TrackMemory>();
            TrackMemoriesWriteDiaries = new HashSet<TrackMemoriesWriteDiary>();
        }

        public string UserId { get; set; }
        public string FirtsName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateBirth { get; set; }
        public int? Gender { get; set; }
        public int? Country { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Friend> FriendFriendUsers { get; set; }
        public virtual ICollection<Friend> FriendUsers { get; set; }
        public virtual ICollection<Notification> NotificationCreationUsers { get; set; }
        public virtual ICollection<Notification> NotificationUsers { get; set; }
        public virtual ICollection<PostUser> PostUsers { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<TrackMemory> TrackMemories { get; set; }
        public virtual ICollection<TrackMemoriesWriteDiary> TrackMemoriesWriteDiaries { get; set; }
    }
}
