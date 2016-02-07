using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Interface.Repository;
using DAL.DALMapper;
using DAL.Interface.Entity;
using System.Data.Entity;
using ORM;

namespace DAL.Repository
{
    public class PictureRepository : Repository<PictureDal, Picture>, IPictureRepository
    {
        public PictureRepository(DbContext context) :
            base(context, DalMapper.PictureToOrm, DalMapper.PictureToDal())
        {

        }

        public override void Update(PictureDal entity)
        {
            if (entity.Id <= 0)
                throw new ArgumentException("The id must be greater than zero");

            Picture changedEntity = DalMapper.PictureToOrm(entity);
            Picture existedEntity = Context.Set<Picture>().FirstOrDefault(en => en.Id == entity.Id);

            if (existedEntity == null)
                throw new ArgumentException(String.Format("The entity with id {0} doesn't exist", entity.Id));

            existedEntity.Image = changedEntity.Image;
            existedEntity.Name = changedEntity.Name;

            Context.Set<Picture>().Attach(existedEntity);
            Context.Entry(existedEntity).State = EntityState.Modified;
        }
    }
}
