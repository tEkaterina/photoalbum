using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DAL.Interface.Entity;

namespace DAL.Interface.Repository
{
    public interface IRepository<TEntity> where TEntity : IDalEntity
    {
        void Create(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);

        IEnumerable<TEntity> GetAll();
        TEntity GetById(int id);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        TEntity GetFirst(Expression<Func<TEntity, bool>> predicate);
    }
}
