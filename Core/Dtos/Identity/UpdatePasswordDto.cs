using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Dtos.Identity
{
    public class UpdatePasswordDto : BasePasswordEntityDto
    {
        [Required(ErrorMessage = "field ({0}) is required")]
        [StringLength(100, ErrorMessage = "The length of the {0} must not be less than {2} letters and not be more than {1} letters.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
    }
}
