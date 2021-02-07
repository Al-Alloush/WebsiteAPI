using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models.Blogs
{
    // BlogCategoryName is the parent of all BlogCategory in any languages
    [Table(name: "BlogSourceCategoryNames")]
    public class BlogSourceCategoryName : BaseBlogModel
    {

        [StringLength(100, ErrorMessage = "The {0} must be less than {1} characters.")]
        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

    }
}
