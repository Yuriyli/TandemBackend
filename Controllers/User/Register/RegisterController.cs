using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.User.Register
{
    [Route("api/register")]
    [ApiController]
    [Tags("User")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class RegisterController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public RegisterController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        [EndpointSummary("Endpoint for client side registration")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(UserLoginReturn),
            Description = "Name and JWT token on retrun"
        )]
        [ProducesResponseType(
            StatusCodes.Status409Conflict,
            Description = "This email already used"
        )]
        public async Task<IActionResult> Post(UserRegister userRegister)
        {
            try
            {
                var searchResult = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Email == userRegister.Email
                );
                if (searchResult != null)
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }

                var newUser = new AppUser
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = userRegister.Name,
                    Email = userRegister.Email,
                    Password = userRegister.Password,
                };

                // Save and check if has changes or send 500 code await _context.Users.AddAsync(newUser);
                await _context.Users.AddAsync(newUser);
                var cnhangesResult = await _context.SaveChangesAsync();
                if (cnhangesResult == 0)
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                var claims = new List<Claim>
                {
                    new Claim("userId", newUser.Id),
                    new Claim(ClaimTypes.Email, newUser.Email),
                    new Claim(ClaimTypes.Name, newUser.Name),
                };

                var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                    signingCredentials: new SigningCredentials(
                        AuthOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256
                    )
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(jwt);
                return Ok(
                    new UserLoginReturn
                    {
                        Id = newUser.Id,
                        Name = newUser.Name,
                        JWTToken = jwtToken,
                    }
                );
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
