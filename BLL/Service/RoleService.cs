using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Interface.Service;
using BLL.Interface.Entity;
using BLL.BLLMapper;
using DAL.Interface;
using DAL.Interface.Repository;

namespace BLL.Service
{
    public class RoleService: IRoleService
    {
        private readonly IUnitOfWork uow;
        private readonly IRoleRepository repository;

        public RoleService(IUnitOfWork uow, IRoleRepository repository)
        {
            this.uow = uow;
            this.repository = repository;
        }

        public RoleBll GetById(int id)
        {
            var roleDal = repository.GetById(id);
            if (roleDal == null)
                return null;
            return roleDal.ToBll();
        }

        public RoleBll GetByName(string name)
        {
            name = name.ToUpper();
            var roleDal = repository.GetFirst(role => role.RoleName == name);
            if (roleDal == null)
                return null;
            return roleDal.ToBll();
        }
        
        public void CreateRole(RoleBll roleBll)
        {
            if (roleBll == null)
                throw new ArgumentNullException("roleBll");

            roleBll.RoleName = roleBll.RoleName.ToUpper();

            var roleDal = repository.GetFirst(role => role.RoleName == roleBll.RoleName);
            if (roleDal != null)
                throw new ArgumentException(String.Format(
                    "The role \"{0}\" already exist", roleBll.RoleName));

            repository.Create(roleBll.ToDal());
            uow.Commit();
        }
        
        public IEnumerable<RoleBll> GetAllRoles()
        {
            return repository.GetAll().Select(role => role.ToBll());
        }
        
        public void AddUserToRole(string role, int userId)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            if (userId <= 0)
                throw new InvalidIdException();

            role = role.ToUpper();
            repository.AddUserToRole(role, userId);
            uow.Commit();
        }

        public void RemoveUserFromRole(string role, int userId)
        {
            if (role == null)
                throw new ArgumentNullException("role");
            if (userId <= 0)
                throw new InvalidIdException();

            role = role.ToUpper();
            repository.RemoveUserFromRole(role, userId);
            uow.Commit();
        }
    }
}
