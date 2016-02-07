using System;
using DAL.Interface.Entity;
using ORM;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.DALMapper
{
    public static class DalMapper
    {
        #region ToDalEntity

        public static Expression<Func<User,UserDal>> UserToDal()
        {
            return user => new UserDal() { 
                Email = user.Email, 
                Password = user.Password, 
                Id = user.Id, 
                Username = user.Username, 
                Salt = user.Salt,
                Roles = user.Roles.Select(role => role.RoleName)
            };
        }

        public static Expression<Func<Picture, PictureDal>> PictureToDal()
        {
            return picture => new PictureDal() { 
                Id = picture.Id, 
                Name = picture.Name, 
                Image = picture.Image,
                Hash = picture.Hash,
            };
        }

        public static Expression<Func<PictureProfile, PictureProfileDal>> PictureProfileToDal()
        {
            return profile => new PictureProfileDal() { 
                Id = profile.Id, 
                Description = profile.Description, 
                LoadingDate = profile.LoadingDate,
                PictureId = profile.Picture.Id, 
                Rating = profile.Rating, 
                UserId = profile.User.Id
            };
        }

        public static Expression<Func<Role, RoleDal>> RoleToDal()
        {
            return role => new RoleDal()
            {
                Description = role.Description,
                Id = role.Id,
                RoleName = role.RoleName
            };
        }

        public static Expression<Func<Avatar, AvatarDal>> AvatarToDal()
        {
            return avatar => new AvatarDal()
            {
                Image = avatar.Image,
                Id = avatar.Id
            };
        }

        #endregion

        #region ToORM

        public static User UserToOrm(UserDal userDal)
        {
            return new User() 
            { 
                Id = userDal.Id, 
                Email = userDal.Email, 
                Password = userDal.Password, 
                Username = userDal.Username, 
                Salt = userDal.Salt
            };
        }

        public static Role RoleToOrm(RoleDal roleDal)
        {
            return new Role()
            {
                Description = roleDal.Description,
                Id = roleDal.Id,
                RoleName = roleDal.RoleName
            };
        }

        public static Picture PictureToOrm(PictureDal pictureDal)
        {
            return new Picture()
            {
                Id = pictureDal.Id,
                Image = pictureDal.Image,
                Name = pictureDal.Name,
                Hash = pictureDal.Hash,
            };
        }

        public static PictureProfile PictureProfileToOrm(PictureProfileDal profileDal)
        {
            return new PictureProfile()
            {
                Description = profileDal.Description,
                Id = profileDal.Id,
                LoadingDate = profileDal.LoadingDate,
                Rating = profileDal.Rating,
                PictureId  = profileDal.PictureId,
                UserId = profileDal.UserId
            };
        }

        public static Avatar AvatarToOrm(AvatarDal avatarDal)
        {
            return new Avatar()
            {
                Id = avatarDal.Id,
                Image = avatarDal.Image,
            };
        }

        #endregion
    }
}
