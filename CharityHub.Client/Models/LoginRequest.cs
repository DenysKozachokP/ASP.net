using System.ComponentModel.DataAnnotations;

namespace CharityHub.Client.Models
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Введіть email")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введіть пароль")]
        [MinLength(6, ErrorMessage = "Мінімальна довжина пароля - 6 символів")]
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}

