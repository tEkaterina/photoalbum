using System;
using DAL.Interface.Entity;

namespace DAL.Interface.Repository
{
    public interface IRoleRepository: IRepository<RoleDal>
    {
        void AddUserToRole(string role, int userId);
        void RemoveUserFromRole(string roleName, int userId);
    }
}
