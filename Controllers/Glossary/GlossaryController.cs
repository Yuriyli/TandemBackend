using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Glossary
{
    [Route("api/glossary/topic")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class GlossaryController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public GlossaryController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointDescription("Get list of topics with selected language")]
        [Route("/api/glossary/titles/{language:alpha}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TopicTitle>))]
        public async Task<IActionResult> GetTopicList(
            [FromRoute] [Description("Accepts only \"en\" and \"ru\"")] Languages language
        )
        {
            try
            {
                List<TopicTitle> result;
                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        result = await _context
                            .Topics.Select(t => new TopicTitle { Id = t.Id, Title = t.TitleRu })
                            .ToListAsync();
                        break;
                    default:
                        result = await _context
                            .Topics.Select(t => new TopicTitle { Id = t.Id, Title = t.Title })
                            .ToListAsync();
                        break;
                }
                return Ok(result);
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{language:alpha}/{id:int}")]
        [EndpointDescription("Get topic with selected language")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TopicMono))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(
            [FromRoute] [Description("Accepts only \"en\" and \"ru\"")] Languages language,
            [FromRoute] int id
        )
        {
            try
            {
                TopicMono? result = null;

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        result = await _context
                            .Topics.Where(t => t.Id == id)
                            .Select(t => new TopicMono
                            {
                                Id = t.Id,
                                Title = t.TitleRu,
                                Content = t.ContentRu,
                            })
                            .FirstOrDefaultAsync();
                        break;
                    default:
                        result = await _context
                            .Topics.Where(t => t.Id == id)
                            .Select(t => new TopicMono
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Content = t.Content,
                            })
                            .FirstOrDefaultAsync();
                        break;
                }

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
        [EndpointDescription("Creates a record in the database with a new topic")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] TopicPut topic)
        {
            try
            {
                await _context.Topics.AddAsync(
                    new Topic
                    {
                        Id = 0,
                        Title = topic.Title,
                        Content = topic.Content,
                        TitleRu = topic.TitleRu,
                        ContentRu = topic.ContentRu,
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
        [EndpointDescription("Delete topic by id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var topic = await _context.Topics.FirstOrDefaultAsync(u => u.Id == id);
                if (topic == null)
                    return NotFound();
                _context.Topics.Remove(topic);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [EndpointDescription("Put changes to topic")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put([FromBody] Topic topic)
        {
            try
            {
                var originTopic = await _context.Topics.FirstOrDefaultAsync(t => t.Id == topic.Id);
                if (originTopic == null)
                    return NotFound();

                originTopic.Title = topic.Title;
                originTopic.Content = topic.Content;
                originTopic.TitleRu = topic.TitleRu;
                originTopic.ContentRu = topic.ContentRu;
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
