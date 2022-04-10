using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class MediaType
    {
        public MediaType()
        {
            PostMediaTypes = new HashSet<PostMediaType>();
        }

        public int MediaTypeId { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<PostMediaType> PostMediaTypes { get; set; }
    }
}
