using BLL.Interface.Entity;
using BLL.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using Ninject;

namespace PhotoAlbum.Providers
{
    public class CustomRoleProvider: RoleProvider
    {
        public override void CreateRole(string roleName)
        {
            var roleService = GetRoleService();
            
            if (roleName == null)
                throw new ArgumentNullException("roleName");

            if (!RoleExists(roleName))
            {
                roleService.CreateRole(new RoleBll() { RoleName = roleName });
            }
        }
        
        public override string[] GetAllRoles()
        {
            var roleService = GetRoleService();
            return roleService.GetAllRoles()
                .Select(role => role.RoleName)
                .ToArray();
        }
        
        public override bool IsUserInRole(string username, string roleName)
        {
            if (username == null)
                throw new ArgumentNullException("username");
            if (roleName == null)
                throw new ArgumentNullException("roleName");

            var userService = GetUserService();
            var user = userService.GetUserEntity(username);
            return user != null && user.Roles.Contains(roleName);
        }
        
        public override string[] GetRolesForUser(string username)
        {
            if (username == null)
                throw new ArgumentNullException("username");

            var userService = GetUserService();
            var user = userService.GetUserEntity(username);

            if (user == null)
                return new string[0];
            return user.Roles.ToArray();
        }

        public override bool RoleExists(string roleName)
        {
            if (roleName == null)
                throw new ArgumentNullException("roleName");

            var roleService = GetRoleService();
            var existRole = roleService.GetByName(roleName);
            return existRole != null;
        }
        
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var userService = GetUserService();
            var roleService = GetRoleService();
            foreach (var username in usernames)
            {
                foreach (var role in roleNames)
                {
                    var user = userService.GetUserEntity(username);
                    roleService.RemoveUserFromRole(role, user.Id);
                }
            }
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var userService = GetUserService();
            var roleService = GetRoleService();
            foreach (var username in usernames)
            {
                foreach (var role in roleNames)
                {
                    var user = userService.GetUserEntity(username);
                    roleService.AddUserToRole(role, user.Id);
                }
            }
        }

        private IUserService GetUserService()
        {
            return (IUserService)System.Web.Mvc.DependencyResolver.Current.GetService<IUserService>();
        }

        private IRoleService GetRoleService()
        {
            return (IRoleService)System.Web.Mvc.DependencyResolver.Current.GetService<IRoleService>();
        }
                        
        #region Not implemented     

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }        
        #endregion

    }
}