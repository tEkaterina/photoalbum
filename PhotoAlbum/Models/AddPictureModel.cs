using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoAlbum.Models
{
    public class AddPictureModel
    {
        [Required]
        [Display(Name="Новое фото")]
        public HttpPostedFileBase ImageFile { get; set; }

        public byte[] Image { get; set; }

        [Required]
        [StringLength(maxNameLength, MinimumLength=minNameLength, 
            ErrorMessage= "Имя должно состоять из 3-30 символов")]
        [Display(Name="Название")]
        public string Name { get; set; }

        [StringLength(maxDescriptionLength, ErrorMessage="Превышена максимальная длина описания")]
        [Display(Name="Описание")]
        public string Description { get; set; }

        public DateTime? CreateDate { get; set; }
        public string Username { get; set; }

        #region constant values
        public const int maxNameLength = 30;
        public const int minNameLength = 3;

        public const int maxDescriptionLength = 500;
        #endregion
    }
}