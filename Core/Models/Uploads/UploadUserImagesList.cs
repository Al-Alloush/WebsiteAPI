﻿using Core.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Models.Uploads
{
    [Table("UploadUserImagesLists")]
    public class UploadUserImagesList
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        [Display(Name = "Path")]
        public string Path { get; set; }

        [Required(ErrorMessage = "{0} is Required")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public bool Default { get; set; }

        public int UploadTypeId { get; set; }
        public UploadType UploadType { get; set; }

    }
}
