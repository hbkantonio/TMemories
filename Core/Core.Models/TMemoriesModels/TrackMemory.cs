using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class TrackMemory
    {
        public TrackMemory()
        {
            Posts = new HashSet<Post>();
            TrackMemoriesWriteDiaries = new HashSet<TrackMemoriesWriteDiary>();
        }

        public string TrackMemoriesId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUserId { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool Active { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<TrackMemoriesWriteDiary> TrackMemoriesWriteDiaries { get; set; }
    }
}
