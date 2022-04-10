using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class TrackMemoriesWriteDiary
    {
        public string TrackMemoriesId { get; set; }
        public string UserId { get; set; }

        public virtual TrackMemory TrackMemories { get; set; }
        public virtual User User { get; set; }
    }
}
