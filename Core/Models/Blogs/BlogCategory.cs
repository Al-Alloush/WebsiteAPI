using Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models.Blogs
{
    [Table(name: "BlogCategories")]
    public class BlogCategory : BaseBlogModel
    {

        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Category Name Id")]
        public int SourceCategoryId { get; set; }
        public virtual BlogSourceCategoryName SourceCategory { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be less than {1} and more than {2} characters.", MinimumLength = 3)]
        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Blog's Language")]
        public string LanguageId { get; set; }
        public virtual Language Language { get; set; }

    }
}
