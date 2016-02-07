using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Interface.Entity;
using DAL.Interface.Repository;
using DAL.DALMapper;
using ORM;
using System.Data.Entity;


namespace DAL.Repository
{
    public class AvatarRepository: Repository<AvatarDal, Avatar>, IAvatarRepository
    {
        public AvatarRepository(DbContext context) :
            base(context, DalMapper.AvatarToOrm, DalMapper.AvatarToDal())
        {

        }

        public override void Update(AvatarDal entity)
        {
            if (entity.Id <= 0)
                throw new ArgumentException("The id must be greater than zero");

            Avatar changedEntity = DalMapper.AvatarToOrm(entity);
            Avatar existedEntity = Context.Set<Avatar>().FirstOrDefault(en => en.Id == entity.Id);

            if (existedEntity == null)
                throw new ArgumentException(String.Format("The entity with id {0} doesn't exist", entity.Id));

            existedEntity.Image = changedEntity.Image;

            Context.Set<Avatar>().Attach(existedEntity);
            Context.Entry(existedEntity).State = EntityState.Modified;
        }
    }
}
