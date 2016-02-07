using System;
using System.Collections.Generic;
using Ninject;
using Ninject.Web.Common;
using System.Data.Entity;
using ORM;
using DAL;
using DAL.Interface;
using DAL.Interface.Repository;
using DAL.Repository;
using BLL.Interface.Service;
using BLL.Service;

namespace DependencyResolver
{
    public static class Resolver
    {
        public static void AddBindings(this IKernel kernel)
        {
            kernel.Bind<DbContext>().To<EntityContext>().InRequestScope();
            kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();

            kernel.Bind<IUserRepository>().To<UserRepository>();
            kernel.Bind<IRoleRepository>().To<RoleRepository>();
            kernel.Bind<IPictureRepository>().To<PictureRepository>();
            kernel.Bind<IPictureProfileRepository>().To<PictureProfileRepository>();
            kernel.Bind<IAvatarRepository>().To<AvatarRepository>();

            kernel.Bind<IUserService>().To<UserService>();
            kernel.Bind<IPictureService>().To<PictureService>();
            kernel.Bind<IAvatarService>().To<AvatarService>();
            kernel.Bind<IRoleService>().To<RoleService>();
            kernel.Bind<IPictureProfileService>().To<PictureProfileService>();
        }
    }
}
