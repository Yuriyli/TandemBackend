using Microsoft.AspNetCore.Mvc;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Task
{
    [Route("api/{language}/task/code-editor")]
    [ApiController]
    [Tags("Tasks")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class CodeEditorController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public CodeEditorController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{name}")]
        [EndpointSummary("Get code-editor task by name of lesson")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CodeEditorGetResult))]
        public async Task<IActionResult> GetCodeEditor(
            [FromRoute] Languages language,
            [FromRoute] string name
        )
        {
            return Ok();
        }

        [HttpPost]
        [Route("/api/{language}/tasks/code-editor/")]
        [EndpointSummary("Create new code-editor task by array of new tasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(
            StatusCodes.Status409Conflict,
            Description = "This name used. Name field should be unique for all lenguages. Try DELETE or PUT."
        )]
        public async Task<IActionResult> Post(
            [FromRoute] Languages language,
            [FromBody] CodeEditorPost[] CodeCompletionPosts
        )
        {
            return Ok();
        }
    }
}
