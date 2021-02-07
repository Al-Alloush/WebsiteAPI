using Core.Models.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models.Identity
{

    [Table(name: "UserSelectedLanguages")]
    public class UserSelectedLanguages
    {
        public int Id { get; set; }

        // this to contain the languages code, like : "ar, de, em,"
        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "User Id")]
        public string UserId { get; set; }

        public AppUser User { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Language Id")]
        public string LanguageId { get; set; }

        public Language Language { get; set; }
    }
}
