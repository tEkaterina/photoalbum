using System;
using System.Collections.Generic;
using BLL.Interface.Entity;
using DAL.Interface.Entity;

namespace BLL.Interface.Service
{
    public interface IPictureService
    {
        PictureBll GetPictureById(int id);
        IEnumerable<PictureBll> GetAllPictures();
        IEnumerable<PictureBll> GetPicturesByHash(string hash);

        void CreatePicture(PictureBll picture);
        void DeletePicture(PictureBll picture);
        void UpdatePicture(PictureBll picture);
    }
}
