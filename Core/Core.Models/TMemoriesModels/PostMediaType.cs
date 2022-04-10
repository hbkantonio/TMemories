using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Models.TMemoriesModels
{
    public partial class PostMediaType
    {
        public int PostId { get; set; }
        public int MediaTypeId { get; set; }
        public string Media { get; set; }

        public virtual MediaType MediaType { get; set; }
        public virtual Post Post { get; set; }
    }
}
