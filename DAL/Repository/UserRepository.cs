using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DAL.DALMapper;
using DAL.Interface.Entity;
using DAL.Interface.Repository;
using ORM;

namespace DAL.Repository
{
    public class UserRepository: Repository<UserDal, User>, IUserRepository
    {
        public UserRepository(DbContext context):
            base(context, DalMapper.UserToOrm, DalMapper.UserToDal())
        {
            
        }

        public override void Update(UserDal entity)
        {
            if (entity.Id <= 0)
                throw new ArgumentException("The id must be greater than zero");

            User changedEntity = DalMapper.UserToOrm(entity);
            User existedEntity = Context.Set<User>().FirstOrDefault(en => en.Id == entity.Id);

            if (existedEntity == null)
                throw new ArgumentException(String.Format("The entity with id {0} doesn't exist", entity.Id));

            existedEntity.Email = changedEntity.Email;
            existedEntity.Password = changedEntity.Password;
            existedEntity.Username = changedEntity.Username;
            existedEntity.Salt = changedEntity.Salt;

            Context.Set<User>().Attach(existedEntity);
            Context.Entry(existedEntity).State = EntityState.Modified;


        }
    }
}
