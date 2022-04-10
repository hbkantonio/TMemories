using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class Post
    {
        public Post()
        {
            PostMediaTypes = new HashSet<PostMediaType>();
            PostUsers = new HashSet<PostUser>();
        }

        public int PostId { get; set; }
        public string TrackMemoriesId { get; set; }
        public string PostedUserId { get; set; }
        public int PostType { get; set; }
        public string Text { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ReadableFrom { get; set; }

        public virtual PostType PostTypeNavigation { get; set; }
        public virtual User PostedUser { get; set; }
        public virtual TrackMemory TrackMemories { get; set; }
        public virtual ICollection<PostMediaType> PostMediaTypes { get; set; }
        public virtual ICollection<PostUser> PostUsers { get; set; }
    }
}
