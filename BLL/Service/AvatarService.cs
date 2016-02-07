using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interface.Entity;
using BLL.Interface.Service;
using DAL.Interface.Entity;
using DAL.Interface.Repository;
using DAL;
using DAL.Interface;
using BLL.BLLMapper;

namespace BLL.Service
{
    public class AvatarService: IAvatarService
    {
        private readonly IUnitOfWork uow;
        private readonly IAvatarRepository repository;

        public AvatarService(IUnitOfWork uow, IAvatarRepository repository)
        {
            this.uow = uow;
            this.repository = repository;
        }
        
        public void CreateAvatar(AvatarBll avatar)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");
                        
            repository.Create(avatar.ToDal());
            uow.Commit();
        }

        public AvatarBll GetById(int id)
        {
            if (id == 0)
                throw new InvalidIdException();
            AvatarDal avatar = repository.GetById(id);
            if (avatar == null)
                return null;
            return avatar.ToBll();
        }

        public void UpdateAvatar(AvatarBll avatar)
        {
            if (avatar == null)
                throw new ArgumentNullException("avatar");

            AvatarDal currentAvatar = avatar.ToDal();
            AvatarDal existedAvatar = repository.GetById(avatar.Id);
            if (existedAvatar == null)
                throw new EntityNotFoundException("avatar", avatar.Id);

            existedAvatar.Image = currentAvatar.Image;

            repository.Update(existedAvatar);
            uow.Commit();
        }
    }
}
