using System;
using System.Collections.Generic;
using System.Linq;
using ORM;
using DAL.DALMapper;
using DAL.Interface.Entity;
using DAL.Interface.Repository;
using System.Data.Entity;

namespace DAL.Repository
{
    public class RoleRepository: Repository<RoleDal, Role>, IRoleRepository
    {
        public RoleRepository(DbContext context) :
            base(context, DalMapper.RoleToOrm, DalMapper.RoleToDal())
        {

        }

        public void AddUserToRole(string roleName, int userId)
        {
            Role existRole = Context.Set<Role>().FirstOrDefault(role => role.RoleName == roleName);
            if (existRole == null)
                throw new ArgumentException(String.Format("The role with name {0} doesn't exist", roleName));

            User existUser = Context.Set<User>().FirstOrDefault(user => user.Id == userId);
            if (existUser == null)
                throw new ArgumentException(String.Format("The user with id {0} doesn't exist", userId));

            existRole.Users.Add(existUser);
        }

        public void RemoveUserFromRole(string roleName, int userId)
        {
            Role existRole = Context.Set<Role>().FirstOrDefault(role => role.RoleName == roleName);
            if (existRole == null)
                throw new ArgumentException(String.Format("The role with name {0} doesn't exist", roleName));

            User existUser = Context.Set<User>().FirstOrDefault(user => user.Id == userId);
            if (existUser == null)
                throw new ArgumentException(String.Format("The user with id {0} doesn't exist", userId));

            existRole.Users.Remove(existUser);
        }

        public override void Update(RoleDal entity)
        {
            if (entity.Id <= 0)
                throw new ArgumentException("The id must be greater than zero");

            Role changedEntity = DalMapper.RoleToOrm(entity);
            Role existedEntity = Context.Set<Role>().FirstOrDefault(en => en.Id == entity.Id);

            if (existedEntity == null)
                throw new ArgumentException(String.Format("The entity with id {0} doesn't exist", entity.Id));

            existedEntity.RoleName = changedEntity.RoleName;
            existedEntity.Description = changedEntity.Description;
            
            Context.Set<Role>().Attach(existedEntity);
            Context.Entry(existedEntity).State = EntityState.Modified;
        }
    }
}
