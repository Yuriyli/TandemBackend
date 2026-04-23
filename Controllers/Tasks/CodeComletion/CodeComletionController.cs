using Microsoft.AspNetCore.Mvc;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Task
{
    [Route("api/{language}/task/code-completion")]
    [ApiController]
    [Tags("Tasks")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CodeCompletionController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CodeCompletionController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{name}")]
        [EndpointSummary("Get quiz by name of lesson")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CodeCompletionGetResult))]
        public async Task<IActionResult> GetCodeCompletion(
            [FromRoute] Languages language,
            [FromRoute] string name
        )
        {
            return Ok();
        }

        [HttpPost]
        [Route("/api/{language}/tasks/code-completion/")]
        [EndpointSummary("Create new quizzes by array of new quizzes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(
            StatusCodes.Status409Conflict,
            Description = "This name used. Name field should be unique for all lenguages. Try DELETE or PUT."
        )]
        public async Task<IActionResult> Post(
            [FromRoute] Languages language,
            [FromBody] CodeCompletionPost[] CodeCompletionPosts
        )
        {
            return Ok();
        }
    }
}
