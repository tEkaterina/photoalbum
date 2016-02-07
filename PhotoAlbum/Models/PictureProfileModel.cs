using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoAlbum.Models
{
    public class PictureProfileModel
    {
        public int Index { get; set; }
        public int PictureId { get; set; }
        public string Image { get; set; } //Base64String
        public string Name { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime? LoadingDate { get; set; }
    }
}