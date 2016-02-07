using BLL.Interface.Service;
using PhotoAlbum.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoAlbum.Infrastructure;
using System.Web.Security;

namespace PhotoAlbum.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IAvatarService avatarService;
        private readonly IRoleService roleService;

        public UserController(IUserService userService, IAvatarService avatarService, IRoleService roleService)
        {
            this.userService = userService;
            this.avatarService = avatarService;
            this.roleService = roleService;
        }

        [HttpPost]
        public ActionResult LoadProfileEditor(string username)
        {
            if (username == null)
                throw new HttpException(404, "Not Found");

            return View("EditProfile", GetUserProfile(username));
        }

        [HttpGet]
        [ActionName("LoadProfileEditor")]
        public ActionResult EditProfile()
        {
            return View("EditProfile", GetUserProfile(User.Identity.Name));
        }

        [HttpPost]
        public ActionResult EditProfile(UserProfileModel profile, List<string> role)
        {
            if (ModelState.IsValid)
            {
                if (profile == null)
                    throw new HttpException(404, "Not Found");

                var provider = new UserMembershipProvider(userService, avatarService, roleService);
                bool isNameChanged = false;

                try
                {
                    if (User.IsInRole("admin"))
                    {
                        isNameChanged = ChangeUsername(provider, profile);
                        provider.ChangeEmail(profile.Username, profile.Email);
                        SetRoles(profile, role);
                    }
                    else
                    {
                        profile.Username = profile.PreviousUsername;
                    }

                    ChangeAvatar(provider, profile);
                    ChangePassword(provider, profile);


                    if (ModelState.IsValid)
                    {
                        TempData["MessageType"] = MessageType.success;
                        TempData["StrongResultMessage"] = "Изменения сохранены";

                        if (isNameChanged && User.Identity.Name == profile.PreviousUsername)
                        {
                            return RedirectToAction("Logout", "Account");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception)
                {
                    TempData["MessageType"] = MessageType.error;
                    TempData["StrongResultMessage"] = "Произошла ошибка во время сохранения изменений!";
                    TempData["ResultMessage"] = "Некоторые данные могут быть не сохранены";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(profile);
        }

        public ActionResult LoadProfile(string username)
        {
            if (username == null)
                throw new HttpException(404, "Not Found");

            UserProfileModel profile = GetUserProfile(username);
            return PartialView(profile);
        }

        private UserProfileModel GetUserProfile(string username)
        {
            var user = userService.GetUserEntity(username);
            if (user == null)
                throw new HttpException(404, "Not Found");

            string avatar;
            if (User.Identity.Name == username && Session["Avatar"] != null)
            {
                avatar = Session["Avatar"] as string;
            }
            else
            {
                avatar = LoadAvatar(username);
                if (User.Identity.Name == username)
                {
                    Session["Avatar"] = avatar;
                }
            }

            return new UserProfileModel() { 
                Avatar = avatar, 
                Email = user.Email, 
                Username = user.Username,
                PreviousUsername = user.Username,
                UserId = user.Id,
                Roles = user.Roles
            };
        }
        
        private string LoadAvatar(string username)
        {
            var user = userService.GetUserEntity(username);
            if (user == null)
                throw new HttpException(404, "Not Found");

            var avatar = avatarService.GetById(user.Id);

            if (avatar == null)
            {
                new UserMembershipProvider(userService, avatarService, roleService)
                    .CreateAvatar(user.Id);
                return LoadAvatar(username);
            }
            else
            {
                return Convert.ToBase64String(avatar.Image);
            }
        }

        private bool ChangeUsername(UserMembershipProvider provider, UserProfileModel profile)
        {
            if (profile.PreviousUsername != profile.Username && !string.IsNullOrEmpty(profile.Username))
            {
                if (!provider.IsUsernameExist(profile.Username))
                {
                    provider.ChangeUsername(profile.PreviousUsername, profile.Username);
                    return true;
                }
                else
                {
                    ModelState.AddModelError("Username", "Такое имя пользователя уже существует");
                }
            }
            return false;
        }

        private void ChangeAvatar(UserMembershipProvider provider, UserProfileModel profile)
        {
            if (profile.AvatarFile != null)
            {
                if (profile.AvatarFile.ContentLength < RegisterUserModel.maxAvatarSize)
                {
                    provider.ChangeUserAvatar(profile.AvatarFile, profile.UserId);
                    Session["Avatar"] = null;
                }
                else
                {
                    ModelState.AddModelError("AvatarFile", 
                        String.Format("Размер аватара не может превышать {0} МБ", 
                            RegisterUserModel.maxAvatarSize / (1024 * 1024)));
                }
            }
        }

        private void ChangePassword(UserMembershipProvider provider, UserProfileModel profile)
        {
            if (profile.Password != null &&
                    !provider.ChangePassword(profile.Username, profile.PreviousPassword, profile.Password))
            {
                ModelState.AddModelError("PreviousPassword", "Неправильный пароль");
            }
        }

        private void SetRoles(UserProfileModel profile, List<string> roles)
        {
            var userRoles = Roles.GetRolesForUser(profile.Username).ToList();
            userRoles.Remove("USER");

            var newRoles = new List<string>();
            newRoles.Add("USER");
            
            if (roles != null)
            {
                foreach (var newRole in roles)
                {
                    if (userRoles.Contains(newRole))
                    {
                        userRoles.Remove(newRole);
                    }
                    else
                    {
                        Roles.AddUserToRole(profile.Username, newRole);
                    }
                }
                newRoles.AddRange(roles);
            }
            foreach (var removedRole in userRoles)
            {
                Roles.RemoveUserFromRole(profile.Username, removedRole);
            }
            profile.Roles = newRoles;
        }
    }
}
