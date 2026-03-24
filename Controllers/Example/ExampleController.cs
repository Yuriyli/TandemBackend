using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Example
{
    [Route("api/example")]
    [ApiController]
    public class ExampleController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public ExampleController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<List<ExampleModel>> Get()
        {
            return await _context.ExampleModels.ToListAsync();
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ExampleModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {
                var result = await _context.ExampleModels.FirstOrDefaultAsync(u => u.Id == id);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(string name, string description)
        {
            try
            {
                await _context.ExampleModels.AddAsync(
                    new ExampleModel
                    {
                        Id = 0,
                        Name = name,
                        Description = description,
                    }
                );
                var isSaved = await _context.SaveChangesAsync();

                if (isSaved >= 1)
                    return Created();
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
            catch
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var model = await _context.ExampleModels.FirstOrDefaultAsync(u => u.Id == id);
                if (model == null)
                    return NotFound();
                _context.ExampleModels.Remove(model);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(ExampleModel model)
        {
            try
            {
                var originModel = await _context.ExampleModels.FirstOrDefaultAsync(u =>
                    u.Id == model.Id
                );
                if (originModel == null)
                    return NotFound();

                originModel.Name = model.Name;
                originModel.Description = model.Description;
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
