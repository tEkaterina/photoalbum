using System;
using System.Data.Entity;

namespace DAL.Interface
{
    public interface IUnitOfWork: IDisposable
    {
        DbContext Context { get; }
        void Commit();
    }
}
