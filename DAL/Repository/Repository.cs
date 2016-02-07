using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Interface.Entity;
using DAL.Interface.Repository;
using DAL.DALMapper;
using ORM;
using System.Data.Entity;
using System.Linq.Expressions;
using LinqKit;
using DAL.Infastructure;

namespace DAL.Repository
{
    public abstract class Repository<TDal, TOrm>: IRepository<TDal>
        where TDal: class, IDalEntity
        where TOrm: class, IEntity
    {
        private Func<TDal, TOrm> ormMapFunc;
        private Expression<Func<TOrm, TDal>> dalMapExpression;

        protected DbContext Context { get; private set; }

        protected Repository(
            DbContext context, 
            Func<TDal, TOrm> ormMapFunc,
            Expression<Func<TOrm, TDal>> dalMapExpression)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            Context = context;

            if (dalMapExpression == null)
                throw new ArgumentNullException("dalMapExpression");

            if (ormMapFunc == null)
                throw new ArgumentNullException("ormMapFunc");

            this.dalMapExpression = dalMapExpression;
            this.ormMapFunc = ormMapFunc;
        }

        public virtual void Create(TDal entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var createdEntity = ormMapFunc(entity);
            if (IsEntityExist(createdEntity))
                throw new ArgumentException("The created entity already exist");

            Context.Set<TOrm>().Add(createdEntity);
        }

        public virtual void Delete(TDal entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            var removedEntity = ormMapFunc(entity);
            if (!IsEntityExist(removedEntity))
                throw new InvalidOperationException("The specified entity doesn't exist");

            Context.Set<TOrm>().Remove(removedEntity);
        }

        public virtual IEnumerable<TDal> Find(Expression<Func<TDal, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            Expression<Func<TOrm, bool>> newPredicate = dalMapExpression.Combine(predicate);

            return Context.Set<TOrm>()
                .Where(newPredicate)
                .Select(dalMapExpression); 
        }

        public virtual TDal GetFirst(Expression<Func<TDal, bool>> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("predicate");
            
            Expression<Func<TOrm, bool>> newPredicate = dalMapExpression.Combine(predicate);

            TOrm foundValue =  Context.Set<TOrm>().FirstOrDefault(newPredicate);
            if (foundValue == null)
                return null;
            return dalMapExpression.Compile()(foundValue);
        }

        public virtual TDal GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("The searching id must be greater than zero");

            return GetFirst(entity => entity.Id == id);
        }

        public virtual IEnumerable<TDal> GetAll()
        {
            return Context.Set<TOrm>().Select(dalMapExpression);
        }

        protected virtual bool IsEntityExist(TOrm entity)
        {
            if (entity.Id != 0)
                return (GetById(entity.Id) != null);
            return false;
        }

        public abstract void Update(TDal entity);
    }
}
