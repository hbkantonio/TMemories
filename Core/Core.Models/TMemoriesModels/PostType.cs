using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class PostType
    {
        public PostType()
        {
            Posts = new HashSet<Post>();
        }

        public int PostTypeId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}
