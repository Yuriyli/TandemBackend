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
        [Route("/api/glossary/topics/{language}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TopicGet>))]
        public async Task<IActionResult> GetTopicList([FromRoute] Languages language)
        {
            try
            {
                List<TopicGet> result;
                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        result = await _context
                            .RuTopics.Select(t => new TopicGet
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Description = t.Description,
                                Example = t.Example,
                            })
                            .ToListAsync();
                        break;
                    default:
                        result = await _context
                            .EnTopics.Select(t => new TopicGet
                            {
                                Id = t.Id,
                                Title = t.Title,
                                Description = t.Description,
                                Example = t.Example,
                            })
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

        [HttpPost]
        [EndpointDescription("Creates a records in the database with a new topics from array")]
        [Route("/api/glossary/topics/{language}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post(
            [FromRoute] Languages language,
            [FromBody] TopicPost[] topics
        )
        {
            try
            {
                foreach (TopicPost topic in topics)
                {
                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:
                            await _context.RuTopics.AddAsync(
                                new RuTopic
                                {
                                    Id = 0,
                                    Title = topic.Title,
                                    Description = topic.Description,
                                    Example = topic.Example,
                                }
                            );

                            break;
                        default:
                            await _context.EnTopics.AddAsync(
                                new EnTopic
                                {
                                    Id = 0,
                                    Title = topic.Title,
                                    Description = topic.Description,
                                    Example = topic.Example,
                                }
                            );

                            break;
                    }
                }

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

        [HttpDelete]
        [EndpointDescription("Delete topics by ids")]
        [Route("/api/glossary/topics/{language}")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(int),
            Description = "Retruns number of deleted topics"
        )]
        public async Task<IActionResult> Delete(
            [FromRoute] Languages language,
            [FromBody] int[] ids
        )
        {
            try
            {
                foreach (var id in ids)
                {
                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:
                            var ruTopic = await _context.RuTopics.FirstOrDefaultAsync(u =>
                                u.Id == id
                            );
                            if (ruTopic != null)
                            {
                                _context.RuTopics.Remove(ruTopic);
                            }
                            break;
                        default:
                            var enTopic = await _context.EnTopics.FirstOrDefaultAsync(u =>
                                u.Id == id
                            );
                            if (enTopic != null)
                            {
                                _context.EnTopics.Remove(enTopic);
                            }

                            break;
                    }
                }

                var saveChangesResult = await _context.SaveChangesAsync();
                return Ok(saveChangesResult);
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [EndpointDescription("Put changes to topic")]
        [Route("/api/glossary/topic/{language}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromRoute] Languages language,
            [FromBody] TopicPut topic
        )
        {
            try
            {
                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        var originRuTopic = await _context.RuTopics.FirstOrDefaultAsync(t =>
                            t.Id == topic.Id
                        );

                        if (originRuTopic == null)
                            return NotFound();

                        originRuTopic.Title = topic.Title;
                        originRuTopic.Description = topic.Description;
                        originRuTopic.Example = topic.Example;
                        break;
                    default:
                        var originEnTopic = await _context.EnTopics.FirstOrDefaultAsync(t =>
                            t.Id == topic.Id
                        );

                        if (originEnTopic == null)
                            return NotFound();

                        originEnTopic.Title = topic.Title;
                        originEnTopic.Description = topic.Description;
                        originEnTopic.Example = topic.Example;
                        break;
                }

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
