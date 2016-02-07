using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityMapper;
using BLL.Interface.Entity;
using PhotoAlbum.Models;

namespace PhotoAlbum.Infrastructure
{
    public static class ModelMapper
    {
        static ModelMapper()
        {
            Mapper.AddRule<RegisterUserModel, UserBll>();
        }

        public static RegisterUserModel ToModel(this UserBll userBll)
        {
            return Mapper.Map<UserBll, RegisterUserModel>(userBll);
        }

        public static UserBll ToBll(this RegisterUserModel userModel)
        {
            return Mapper.Map<RegisterUserModel, UserBll>(userModel);
        }
    }
}