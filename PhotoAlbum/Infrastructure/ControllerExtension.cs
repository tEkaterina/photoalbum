using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhotoAlbum.Infrastructure
{
    public enum MessageType { success, error, info }

    public static class ControllerExtension
    {
        public static byte[] GetBytes(this HttpPostedFileBase image)
        {
            byte[] imageData;
            using (var binaryReader = new BinaryReader(image.InputStream))
            {
                imageData = binaryReader.ReadBytes(image.ContentLength);
            }
            return imageData;
        }
    }
}