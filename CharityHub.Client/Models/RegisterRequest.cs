using System.ComponentModel.DataAnnotations;

namespace CharityHub.Client.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Вкажіть ім'я")]
        [StringLength(50, ErrorMessage = "Максимум 50 символів")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть прізвище")]
        [StringLength(50, ErrorMessage = "Максимум 50 символів")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть email")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть пароль")]
        [MinLength(8, ErrorMessage = "Мінімум 8 символів")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Підтвердіть пароль")]
        [Compare(nameof(Password), ErrorMessage = "Паролі не співпадають")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

