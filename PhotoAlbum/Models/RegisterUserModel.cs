using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PhotoAlbum.Models
{
    [Flags]
    public enum Role 
    { 
        User = 0x01, 
        Admin = 0x02
    }

    public class RegisterUserModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage="Введите имя пользователя")]
        [StringLength(maxUsernameLength, MinimumLength = minUsernameLength,
            ErrorMessage = "Имя пользователя должно быть от 4 до 20 символов")]
        [RegularExpression(@"[\w_]+", 
            ErrorMessage="Имя пользователя может состоять только из символов: A-Z, a-z, 0-9, _")]
        [Display(Name="Имя пользователя")]
        public string Username { get; set; }
        
        [Required(ErrorMessage="Введите email")]
        [EmailAddress(ErrorMessage="Введите корректный email")]
        public string Email { get; set; }
        
        [Required(ErrorMessage="Введите пароль")]
        [StringLength(maxPasswordLength, MinimumLength = minPasswordLength,
            ErrorMessage = "Пароль должен состоять из 6-30 символов")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [StringLength(maxPasswordLength, MinimumLength = minPasswordLength,
            ErrorMessage = "Пароль должен состоять из 6-30 символов")]
        [Display(Name = "Повторите пароль")]
        public string ConfirmPassword { get; set; }
        
        public Role Role { get; set; }

        [Display(Name="Аватар")]
        public HttpPostedFileBase Avatar { get; set; }

        #region static values

        public const int maxAvatarSize = 2097152;

        public const int minUsernameLength = 4;
        public const int maxUsernameLength = 20;

        public const int minPasswordLength = 6;
        public const int maxPasswordLength = 30;

        #endregion
    }
}