using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;

namespace TandemBackend.Controllers.Stat
{
    [Route("api/stat/user")]
    [ApiController]
    [Tags("Stat")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public class StatUserController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public StatUserController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/api/stat/user/score")]
        [EndpointSummary("Get user total score")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(int),
            Description = "User total score from tasks"
        )]
        [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User id not found")]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return NotFound("User id not found");
                }
                string userId = userIdClaim.Value;
                var result = await _context
                    .TaskStats.Where(ts => ts.UserId == userId)
                    .SumAsync(ts => ts.EarnedPoints);
                return Ok(result);
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
