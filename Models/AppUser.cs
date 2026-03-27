using System.ComponentModel.DataAnnotations;

namespace TandemBackend.Models
{
    public class AppUser
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UserLogin
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class UserRegister
    {
        public required string Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid emal format")]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
