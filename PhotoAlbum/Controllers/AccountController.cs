using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PhotoAlbum.Models;
using PhotoAlbum.Infrastructure;
using System.IO;
using BLL.Interface.Service;
using BLL.Interface.Entity;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using PhotoAlbum.Providers;

namespace PhotoAlbum.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserMembershipProvider membershipProvider;

        public AccountController(IUserService userService, IAvatarService avatarService, IRoleService roleService)
        {
            membershipProvider = new UserMembershipProvider(userService, avatarService, roleService);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            if (!string.IsNullOrEmpty(Request.QueryString.ToString()))
            {
                return RedirectToAction("Register");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Register(RegisterUserModel user)
        {
            if (membershipProvider.IsUsernameExist(user.Username))
            {
                ModelState.AddModelError("Username", "Такое имя пользователя уже существует");
            }

            if (user.Avatar != null && user.Avatar.ContentLength > RegisterUserModel.maxAvatarSize)
            {
                ModelState.AddModelError("Avatar", String.Format("Размер аватара не может превышать {0} МБ", RegisterUserModel.maxAvatarSize / (1024 * 1024)));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    membershipProvider.CreateUser(user);
                    TempData["MessageType"] = MessageType.success;
                    TempData["StrongResultMessage"] = "Вы успешно зарегистрированы!";
                    TempData["ResultMessage"] = "Используйте ваш логин и пароль для входа.";
                }
                catch (Exception)
                {
                    TempData["MessageType"] = MessageType.error;
                    TempData["StrongResultMessage"] = "Регистрация не удалась";
                }
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginUserModel user)
        {
            if (ModelState.IsValid)
            {
                if (membershipProvider.ValidateUser(user.Username, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.Username, !user.NotRememberMe);
                    return RedirectToAction("Index", "Home");
                }
                {
                    ModelState.AddModelError("LoginResult", "Неверный логин или пароль");
                }
            }
            return View(user);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["Avatar"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}
