using Core.Models.Blogs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models.Uploads
{
    [Table(name: "UploadBlogImagesList")]
    public class UploadBlogImagesList
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; }

        public int UploadId { get; set; }
        public Upload Upload { get; set; }

        public bool Default { get; set; }

        public int UploadTypeId { get; set; }
        public UploadType UploadType { get; set; }
    }
}
