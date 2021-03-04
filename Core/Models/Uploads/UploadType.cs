using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models.Uploads
{
    [Table("UploadTypes")]
    public class UploadType
    {
        public int Id { get; set; }

        // Defult Type: ImageProfile, ImageCover, ImageBlog
        public string Name { get; set; }
        public virtual ICollection<UploadUserImagesList> UploadUserImagesList { get; set; }
    }
}
