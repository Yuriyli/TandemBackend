using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Stat
{
    [Route("api/stat/user/task")]
    [ApiController]
    [Tags("Stat")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public class StatUserTaskController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public StatUserTaskController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Get list of task statistics for current user")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(List<TaskStatGetResult>),
            Description = "Array of TaskStatResult or empty array"
        )]
        [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User id not found")]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                // Get user id in db
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return NotFound("User id not found");
                }
                string userId = userIdClaim.Value;

                var searchResult = await _context
                    .TaskStats.Where(ts => ts.UserId == userId && ts.IsFinished)
                    .Select(ts => new TaskStatGetResult
                    {
                        TaskId = ts.TaskId,
                        TaskType = ts.TaskType,
                        TaskDifficulty = ts.TaskDifficulty,
                    })
                    .ToListAsync();
                return Ok(searchResult ?? new List<TaskStatGetResult>());
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [EndpointSummary("Post a task statistics record for the user")]
        [ProducesResponseType(
            StatusCodes.Status201Created,
            Description = "Returned if new entry is made"
        )]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Description = "Retruned if entry was in db, and changes IsFinished status"
        )]
        [ProducesResponseType(StatusCodes.Status404NotFound, Description = "User id not found")]
        public async Task<IActionResult> PostTask(TaskStatPost taskStatPost)
        {
            try
            {
                var userIdClaim = User.FindFirst("userId");
                if (userIdClaim == null)
                {
                    return NotFound("User id not found");
                }
                string userId = userIdClaim.Value;

                // Check if entry alrdy in db
                var searchResult = await _context.TaskStats.FirstOrDefaultAsync(ts =>
                    ts.UserId == userId
                    && ts.TaskId == taskStatPost.TaskId
                    && ts.TaskDifficulty == taskStatPost.TaskDifficulty
                    && ts.TaskType == taskStatPost.TaskType
                );
                if (searchResult != null)
                {
                    searchResult.IsFinished = taskStatPost.IsFinished;
                    var saveResult = await _context.SaveChangesAsync();

                    return Ok();
                }

                var newTaskStat = new TaskStat
                {
                    Id = 0,
                    UserId = userId,
                    TaskId = taskStatPost.TaskId,
                    IsFinished = taskStatPost.IsFinished,
                    TaskType = taskStatPost.TaskType,
                    TaskDifficulty = taskStatPost.TaskDifficulty,
                };

                await _context.TaskStats.AddAsync(newTaskStat);
                var saveChangesResult = await _context.SaveChangesAsync();

                if (saveChangesResult <= 0)
                {
                    Console.WriteLine($"Failed to save new task user id: {userId}");
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }

                return Created();
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
