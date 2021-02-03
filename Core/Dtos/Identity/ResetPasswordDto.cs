using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Dtos.Identity
{
    public class ResetPasswordDto : BasePasswordEntityDto
    {
        [Required(ErrorMessage = "field ({0}) is required")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "field ({0}) is required")]
        public string Token { get; set; }
    }
}
