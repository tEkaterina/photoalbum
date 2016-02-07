using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoAlbum.Models
{
    public class UserProfileModel
    {
        [StringLength(RegisterUserModel.maxUsernameLength, MinimumLength = RegisterUserModel.minUsernameLength,
            ErrorMessage = "Имя пользователя должно быть от 4 до 20 символов")]
        [RegularExpression(@"[\w_]+",
            ErrorMessage = "Имя пользователя может состоять только из символов: A-Z, a-z, 0-9, _")]
        [Display(Name = "Имя пользователя")]
        public string Username { get; set; }

        public string PreviousUsername { get; set; }

        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; }

        [StringLength(RegisterUserModel.maxPasswordLength, MinimumLength = RegisterUserModel.minPasswordLength,
            ErrorMessage = "Пароль должен состоять из 6-30 символов")]
        [Display(Name = "Предыдущий пароль")]
        public string PreviousPassword { get; set; }

        [StringLength(RegisterUserModel.maxPasswordLength, MinimumLength = RegisterUserModel.minPasswordLength,
            ErrorMessage = "Пароль должен состоять из 6-30 символов")]
        [Display(Name = "Новый пароль")]
        public string Password { get; set; }

        [StringLength(RegisterUserModel.maxPasswordLength, MinimumLength = RegisterUserModel.minPasswordLength,
            ErrorMessage = "Пароль должен состоять из 6-30 символов")]
        [Display(Name = "Повторите новый пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Файл изображения")]
        public HttpPostedFileBase AvatarFile { get; set; }

        public IEnumerable<string> Roles { get; set; }

        public string Avatar { get; set; } //Base64String

        public int UserId { get; set; }
    }
}