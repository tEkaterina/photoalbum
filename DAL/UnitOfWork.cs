using System;
using System.Collections.Generic;
using System.Linq;
using ORM;
using DAL.Interface;
using System.Data.Entity;

namespace DAL
{
    public class UnitOfWork: IUnitOfWork
    {
        public DbContext Context { get; private set; }
        private bool isDisposed;

        public UnitOfWork(DbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            isDisposed = false;

            Context = context;
        }
        
        public void Commit()
        {
            if (isDisposed)
                throw new ObjectDisposedException("Unit of work");

            var result = Context.SaveChanges();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                Context.Dispose();
            }
        }
    }
}
