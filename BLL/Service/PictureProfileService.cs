using DAL.Interface;
using DAL.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Service
{
    using Interface.Entity;
    using Interface.Service;
    using BLLMapper;
    using DAL.Interface.Entity;

    public class PictureProfileService: IPictureProfileService
    {
        private readonly IUnitOfWork uow;
        private readonly IPictureProfileRepository repository;

        public PictureProfileService(IUnitOfWork uow, IPictureProfileRepository repository)
        {
            this.uow = uow;
            this.repository = repository;
        }

        public PictureProfileBll GetProfileById(int id)
        {
            if (id <= 0)
                throw new InvalidIdException();

            var profileDal = repository.GetById(id);
            if (profileDal == null)
                throw new EntityNotFoundException("profile profile", id);
            return profileDal.ToBll();

        }

        public IEnumerable<PictureProfileBll> GetAllProfiles()
        {
            return repository.GetAll().Select(profile => profile.ToBll());
        }

        public IEnumerable<PictureProfileBll> GetAllUsersProfiles(int userId)
        {
            if (userId <= 0)
                throw new InvalidIdException();
            return repository
                    .Find(profile => profile.UserId == userId)
                    .Select(profile => profile.ToBll());                
        }

        public void CreatePictureProfile(PictureProfileBll profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            repository.Create(profile.ToDal());
            uow.Commit();
        }

        public void DeletePictureProfile(PictureProfileBll profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            if (profile.Id <= 0)
                throw new InvalidIdException();
            
            var existedProfile = repository.GetById(profile.Id);
            if (existedProfile == null)
                throw new ArgumentException("The profile cannot be found");

            repository.Delete(existedProfile);
            uow.Commit();
        }

        public void UpdatePictureProfile(PictureProfileBll profile)
        {
            if (profile == null)
                throw new ArgumentNullException("profile");

            PictureProfileDal currentPictureProfile = profile.ToDal();
            PictureProfileDal existedPictureProfile = repository.GetById(profile.Id);
            if (existedPictureProfile == null)
                throw new EntityNotFoundException("profile", profile.Id);

            existedPictureProfile.Description = currentPictureProfile.Description;
            existedPictureProfile.PictureId = currentPictureProfile.PictureId;
            existedPictureProfile.Rating = currentPictureProfile.Rating;

            repository.Update(existedPictureProfile);
            uow.Commit();

        }
    }
}
