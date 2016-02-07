using System;
using EntityMapper;
using BLL.Interface.Entity;
using DAL.Interface.Entity;

namespace BLL.BLLMapper
{
    public static class BllMapper
    {
        static BllMapper()
        {
            Mapper.AddRule<UserBll, UserDal>();
            Mapper.AddRule<PictureBll, PictureDal>();
            Mapper.AddRule<PictureProfileBll, PictureProfileDal>();
            Mapper.AddRule<RoleBll, RoleDal>();
            Mapper.AddRule<AvatarDal, AvatarBll>();
        }

        public static UserBll ToBll(this UserDal userDal)
        {
            return Mapper.Map<UserDal, UserBll>(userDal);
        }

        public static UserDal ToDal(this UserBll userBll)
        {
            return Mapper.Map<UserBll, UserDal>(userBll);
        }

        public static PictureBll ToBll(this PictureDal pictureDal)
        {
            return Mapper.Map<PictureDal, PictureBll>(pictureDal);
        }

        public static PictureDal ToDal(this PictureBll pictureBll)
        {
            return Mapper.Map<PictureBll, PictureDal>(pictureBll);
        }

        public static AvatarBll ToBll(this AvatarDal avatarDal)
        {
            return Mapper.Map<AvatarDal, AvatarBll>(avatarDal);
        }

        public static AvatarDal ToDal(this AvatarBll avatarBll)
        {
            return Mapper.Map<AvatarBll, AvatarDal>(avatarBll);
        }

        public static RoleDal ToDal(this RoleBll roleBll)
        {
            return Mapper.Map<RoleBll, RoleDal>(roleBll);
        }

        public static RoleBll ToBll(this RoleDal roleDal)
        {
            return Mapper.Map<RoleDal, RoleBll>(roleDal);
        }

        public static PictureProfileBll ToBll(this PictureProfileDal profileDal)
        {
            return Mapper.Map<PictureProfileDal, PictureProfileBll>(profileDal);
        }

        public static PictureProfileDal ToDal(this PictureProfileBll profileBll)
        {
            return Mapper.Map<PictureProfileBll, PictureProfileDal>(profileBll);
        }
    }
}
