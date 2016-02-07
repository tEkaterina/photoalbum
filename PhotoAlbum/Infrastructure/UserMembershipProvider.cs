using BLL.Interface.Entity;
using BLL.Interface.Service;
using PhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.Security;

namespace PhotoAlbum.Infrastructure
{
    public class UserMembershipProvider
    {
        private readonly IUserService userService;
        private readonly IAvatarService avatarService;
        private readonly IRoleService roleService;

        public UserMembershipProvider(IUserService userService, IAvatarService avatarService, IRoleService roleService)
        {
            this.userService = userService;
            this.avatarService = avatarService;
            this.roleService = roleService;
        }
        
        public void CreateUser(RegisterUserModel user)
        {
            var bllUser = user.ToBll();

            bllUser.Salt = GenerateSalt();
            bllUser.Password = ComputePasswordHash(user.Password, bllUser.Salt);
            userService.CreateUser(bllUser);

            Roles.AddUserToRole(user.Username, "user");
            var addedUser = userService.GetUserEntity(user.Username);

            CreateAvatar(addedUser.Id, user.Avatar);
        }
        
        public void ChangeUserAvatar(HttpPostedFileBase avatar, int userId)
        {
            var newAvatar = new AvatarBll()
            {
                Image = GetAvatar(avatar),
                Id = userId
            };
            avatarService.UpdateAvatar(newAvatar);
        }

        public void CreateAvatar(int userId, HttpPostedFileBase avatar = null)
        {
            var newAvatar = new AvatarBll()
            {
                Image = GetAvatar(avatar),
                Id = userId
            };
            avatarService.CreateAvatar(newAvatar);  
        }

        public void ChangeUsername(string username, string newUsername)
        {
            if (username != newUsername)
            {
                var existUser = userService.GetUserEntity(username);
                if (existUser == null)
                    throw new ArgumentException(String.Format("The user '{0}' doesn't exist", username));

                existUser.Username = newUsername;
                userService.UpdateUser(existUser);
            }
        }

        public void ChangeEmail(string username, string newEmail)
        {
            var existUser = userService.GetUserEntity(username);
            if (existUser == null)
                throw new ArgumentException(String.Format("The user {0} doesn't exist", username));
            if (existUser.Email != newEmail)
            {
                existUser.Email = newEmail;
                userService.UpdateUser(existUser);
            }
        }

        public bool ChangePassword(string username, string prevPass, string newPass)
        {
            var user = userService.GetUserEntity(username);
            if (user == null)
                throw new ArgumentException("The specified user doesn't exist");
            if (!ValidateUser(username, prevPass))
                return false;

            user.Password = ComputePasswordHash(newPass, user.Salt);
            userService.UpdateUser(user);

            return true;
        }
        
        public bool IsUsernameExist(string username)
        {
            var userBll = userService.GetUserEntity(username);
            return userBll != null;
        }

        public bool ValidateUser(string username, string password)
        {
            if (!string.IsNullOrEmpty(username))
            {
                var existUser = userService.GetUserEntity(username);

                return existUser != null &&
                       existUser.Password == ComputePasswordHash(password, existUser.Salt);
            }
            return false;
        }

        private byte[] GetAvatar(HttpPostedFileBase avatar)
        {
            if (avatar != null)
            {
                byte[] imageData = avatar.GetBytes();

                if (imageData.Length > RegisterUserModel.maxAvatarSize)
                {
                    return null;
                }
                return imageData;
            }
            else
                return GetDefaultAvatar();
        }
                
        private byte[] GetDefaultAvatar()
        {
            var image = Properties.Resources.defaultAvatar;
            byte[] byteArray;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

        private string GenerateSalt()
        {
            var random = new Random(DateTime.Now.Millisecond);

            int minLength = 7, maxLength = 9;
            var saltLength = random.Next(minLength, maxLength);

            var maxNumber = (int)(Math.Pow(10, saltLength) - 1);
            var minNumber = (int)(Math.Pow(10, saltLength - 1));

            string salt = random.Next(minNumber, maxNumber).ToString();

            return salt;
        }

        private string ComputePasswordHash(string password, string salt)
        {
            var sha256 = new SHA256Cng();
            password += salt;

            byte[] hash = sha256.ComputeHash(Encoding.Default.GetBytes(password));

            return BitConverter.ToString(hash, 0);
        }
    }
}