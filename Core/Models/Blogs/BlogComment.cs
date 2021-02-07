using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models.Blogs
{
    [Table(name: "BlogComments")]
    public class BlogComment : BaseBlogModel
    {

        [Required(ErrorMessage = "{0} is Required")]
        public DateTime AddedDateTime { get; set; }

        [StringLength(255, ErrorMessage = "The {0} must be less than {1} and more than {2} characters.", MinimumLength = 3)]
        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public int BlogId { get; set; }
        public virtual Blog Blog { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
