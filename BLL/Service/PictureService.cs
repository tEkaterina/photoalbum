using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BLL.Interface.Service;
using BLL.Interface.Entity;
using BLL.BLLMapper;
using DAL.Interface;
using DAL.Interface.Repository;
using DAL.Interface.Entity;

namespace BLL.Service
{
    public class PictureService: IPictureService
    {
        private IPictureRepository repository;
        private IUnitOfWork uow;

        public PictureService(IUnitOfWork uow, IPictureRepository repository)
        {
            this.repository = repository;
            this.uow = uow;
        }
        
        public IEnumerable<PictureBll> GetAllPictures()
        {
            return repository.GetAll().Select(pic => pic.ToBll());
        }

        public void CreatePicture(PictureBll picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            var pictureDal = picture.ToDal();
            repository.Create(pictureDal);
            uow.Commit();
        }

        public void DeletePicture(PictureBll picture)
        {
            if (picture == null) throw new ArgumentNullException("picture");
            if (picture.Id <= 0)
                throw new InvalidIdException();

            var removedPicture = repository.GetById(picture.Id);

            if (removedPicture == null) throw
                new EntityNotFoundException("picture", picture.Id);

            repository.Delete(removedPicture);
            uow.Commit();
        }

        public void UpdatePicture(PictureBll picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            PictureDal currentPicture = picture.ToDal();
            PictureDal existedPicture = repository.GetById(picture.Id);
            if (existedPicture == null)
                throw new EntityNotFoundException("picture", picture.Id);

            existedPicture.Image = currentPicture.Image;
            existedPicture.Hash = currentPicture.Hash;
            existedPicture.Name = currentPicture.Name;

            repository.Update(existedPicture);
            uow.Commit();
        }

        public PictureBll GetPictureById(int id)
        {
            if (id <= 0)
                throw new InvalidIdException();

            PictureDal foundPicture = repository.GetById(id);
            if (foundPicture == null)
                return null;
            return foundPicture.ToBll();
        }

        public IEnumerable<PictureBll> GetPicturesByHash(string hash)
        {
            return repository.Find(image => image.Hash == hash).Select(image => image.ToBll());
        }
    }
}
