using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM
{
    public class EntityContext: DbContext
    {
        static EntityContext()
        {
            Database.SetInitializer<EntityContext>(new PhotoAlbumDbInitializer());
        }

        public EntityContext()
            : base("PhotoAlbumDB")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<PictureProfile> PictureProfiles { get; set; }
        public DbSet<Avatar> Avatar { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
