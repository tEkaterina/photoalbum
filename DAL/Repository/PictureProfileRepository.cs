using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Interface.Entity;
using DAL.Interface.Repository;
using ORM;
using DAL.DALMapper;
using System.Data.Entity;


namespace DAL.Repository
{
    public class PictureProfileRepository: Repository<PictureProfileDal, PictureProfile>, IPictureProfileRepository
    {
        public PictureProfileRepository(DbContext context) :
            base(context, DalMapper.PictureProfileToOrm, DalMapper.PictureProfileToDal())
        {

        }
        public override void Update(PictureProfileDal entity)
        {
            if (entity.Id <= 0)
                throw new ArgumentException("The id must be greater than zero");

            PictureProfile changedEntity = DalMapper.PictureProfileToOrm(entity);
            PictureProfile existedEntity = Context.Set<PictureProfile>().FirstOrDefault(en => en.Id == entity.Id);

            if (existedEntity == null)
                throw new ArgumentException(String.Format("The entity with id {0} doesn't exist", entity.Id));

            existedEntity.Description = changedEntity.Description;
            existedEntity.PictureId = changedEntity.PictureId;
            existedEntity.Rating = changedEntity.Rating;
            
            Context.Set<PictureProfile>().Attach(existedEntity);
            Context.Entry(existedEntity).State = EntityState.Modified;
        }
    }
}
