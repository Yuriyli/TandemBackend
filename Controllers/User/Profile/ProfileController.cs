using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.User.Login
{
    [Route("api/user/profile")]
    [ApiController]
    [Tags("User")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ProfileController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpPut]
        [EndpointSummary("Change user profile data")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserProfilePutReturn))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User not found")]
        [ProducesResponseType(StatusCodes.Status409Conflict, Description = "Email already used")]
        public async Task<IActionResult> ProfilePut([FromBody] UserProfilePut userPut)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return NotFound("User id not found");
                }
                string userId = userIdClaim.Value;

                var userSearchResult = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Id == userId
                );
                if (userSearchResult == null)
                {
                    return NotFound("User not found");
                }

                var dublicateEmailSearch = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Email == userPut.Email && u.Id != userId
                );
                if (dublicateEmailSearch != null)
                {
                    return Conflict("This Email already used");
                }

                userSearchResult.Name = userPut.Name;
                userSearchResult.Email = userPut.Email;

                await _context.SaveChangesAsync();

                return Ok(
                    new UserProfilePutReturn
                    {
                        Name = userSearchResult.Name,
                        Email = userSearchResult.Email,
                    }
                );
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("/api/user/profile/password")]
        [EndpointSummary("Change user password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User not found")]
        public async Task<IActionResult> PasswordPut([FromBody] UserPasswordPut passwordPut)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return NotFound("User id not found");
                }
                string userId = userIdClaim.Value;

                var userSearchResult = await _context.Users.FirstOrDefaultAsync(u =>
                    u.Id == userId
                );
                if (userSearchResult == null)
                {
                    return NotFound("User not found");
                }

                if (userSearchResult.Password != passwordPut.PreviousPassword)
                {
                    return Unauthorized("Wrong password");
                }

                userSearchResult.Password = passwordPut.NewPassword;

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
