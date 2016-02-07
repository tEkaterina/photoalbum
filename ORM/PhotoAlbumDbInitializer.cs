using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Drawing;
using System.IO;

namespace ORM
{
    public class PhotoAlbumDbInitializer: DropCreateDatabaseIfModelChanges<EntityContext>
    {
        protected override void Seed(EntityContext context)
        {
            var adminRole = new Role() { RoleName = "ADMIN" };
            var userRole = new Role() { RoleName = "USER" };
            Role[] roles = { adminRole, userRole };
            context.Set<Role>().AddRange(roles);

            var user = new User()
            {
                Username = "super_admin",
                Email = "admin@mail.ru",
                Salt = "98202291",
                Password = "F4-D3-BD-8C-7A-15-41-0F-E5-B8-EE-E7-C5-4E-66-CE-A0-EE-FF-48-CF-E5-5E-1C-C2-4A-84-DD-C4-E5-DC-B9",
            };

            context.Set<User>().Add(user);
            adminRole.Users.Add(user);
            userRole.Users.Add(user);

            base.Seed(context);
        }
    }
}
