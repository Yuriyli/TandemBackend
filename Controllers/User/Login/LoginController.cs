using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.User.Login
{
    [Route("api/login")]
    [ApiController]
    [Tags("User")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public LoginController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPost]
        [EndpointSummary("Endpoint for client side login")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(UserLoginReturn),
            Description = "Name and JWT token on retrun"
        )]
        [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User not found")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Description = "Wrong password")]
        public async Task<IActionResult> Post(UserLogin userLogin)
        {
            try
            {
                var searchResult = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Email == userLogin.Email
                );

                if (searchResult == null)
                {
                    return new StatusCodeResult(StatusCodes.Status404NotFound);
                }
                if (searchResult.Password != userLogin.Password)
                {
                    Console.WriteLine(searchResult.Password);
                    Console.WriteLine(userLogin.Password);
                    return Unauthorized("Wrong password or email");
                }

                var claims = new List<Claim>
                {
                    new Claim("userId", searchResult.Id),
                    new Claim(ClaimTypes.Email, searchResult.Email),
                    new Claim(ClaimTypes.Name, searchResult.Name),
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
                        Id = searchResult.Id,
                        Name = searchResult.Name,
                        JWTToken = jwtToken,
                    }
                );
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
