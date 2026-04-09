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
        [Required]
        [MinLength(3)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid emal format")]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
    }

    public class UserLoginReturn
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string JWTToken { get; set; }
    }

    public class UserProfilePut
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public class UserProfilePutReturn
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    public class UserPasswordPut
    {
        public required string PreviousPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}
