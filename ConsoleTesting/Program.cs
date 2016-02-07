using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using EntityMapper;
using DAL.Interface.Entity;
using ORM;
using BLL.Interface.Entity;
using BLL.Interface.Service;
using BLL.Service;
using DAL.Interface.Repository;
using DAL.Repository;
using DAL;
using System.Data.Entity;
using System.Drawing;
using System.IO;

namespace ConsoleTesting
{
    class Program
    {
        static void Main(string[] args)
        {
            DbContext context = new EntityContext();
            var uow = new UnitOfWork(context);

            var userservice = new UserService(uow, new UserRepository(context));
            var roleservice = new RoleService(uow, new RoleRepository(context));
            var pictureService = new PictureService(uow, new PictureRepository(context));
            var picProfileService = new PictureProfileService(uow, new PictureProfileRepository(context));

            var user = userservice.GetUserEntity("super_admin");
            //userservice.DeleteUser(user);
            roleservice.AddUserToRole("admin", user.Id);

            File.WriteAllLines("users.txt", new string[] { user.Username, user.Salt, user.Password, user.Email});
        }
    }
}
