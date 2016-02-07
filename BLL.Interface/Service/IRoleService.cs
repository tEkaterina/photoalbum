using System;
using BLL.Interface.Entity;
using System.Collections.Generic;

namespace BLL.Interface.Service
{
    public interface IRoleService
    {
        RoleBll GetById(int id);
        RoleBll GetByName(string name);
        IEnumerable<RoleBll> GetAllRoles();

        void CreateRole(RoleBll role);
        void AddUserToRole(string role, int user);
        void RemoveUserFromRole(string role, int user);
    }
}
