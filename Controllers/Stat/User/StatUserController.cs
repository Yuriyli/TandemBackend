using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

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
            Type = typeof(ScoreReturn),
            Description = "User total score from tasks"
        )]
        [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User id not found")]
        public async Task<IActionResult> GetScore()
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return NotFound("User id not found");
                }
                string userId = userIdClaim.Value;

                var userDb = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (userDb == null)
                {
                    return NotFound("User id not found");
                }

                var score = userDb.TotalScore;

                var ratingList = await _context
                    .Users.OrderByDescending(u => u.TotalScore)
                    .Select(u => u.Id)
                    .ToListAsync();

                var userRating = ratingList.FindIndex(id => id == userId) + 1;

                var result = new ScoreReturn { Score = score, UserRating = userRating };
                return Ok(result);
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
