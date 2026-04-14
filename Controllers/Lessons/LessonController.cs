using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Lessons
{
    [Route("api/{language}/lessons")]
    [ApiController]
    [Tags("Lessons")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class LessonController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public LessonController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [EndpointSummary("Get list of lessons with selected language")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LessonGetResult>))]
        public async Task<IActionResult> Get([FromRoute] Languages language)
        {
            try
            {
                List<LessonGetResult> result;
                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        var lessonsList = await _context
                            .Lessons.Include(l => l.LessonRu)
                            .ToListAsync();
                        result = new List<LessonGetResult>();
                        foreach (var lesson in lessonsList)
                        {
                            if (lesson.LessonRu != null)
                            {
                                result.Add(
                                    new LessonGetResult
                                    {
                                        Id = lesson.Id,
                                        Name = lesson.Name,
                                        Title = lesson.LessonRu.Title,
                                    }
                                );
                            }
                        }
                        break;
                    default:
                        result = await _context
                            .Lessons.Select(l => new LessonGetResult
                            {
                                Id = l.Id,
                                Name = l.Name,
                                Title = l.Title,
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
        [EndpointSummary("Create new lessons by array of ne lessons")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(
            StatusCodes.Status409Conflict,
            Description = "This name used. Name field should be unique for all lenguages. Try DELETE or PUT."
        )]
        public async Task<IActionResult> Post(
            [FromRoute] Languages language,
            [FromBody] LessonPost[] lessonPosts
        )
        {
            try
            {
                foreach (var lessonPost in lessonPosts)
                {
                    var lessonDb = await _context
                        .Lessons.Where(l => l.Name == lessonPost.Name)
                        .Include(l => l.LessonRu)
                        .FirstOrDefaultAsync();

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:

                            if (lessonDb == null)
                            {
                                return NotFound("Create english version before other language");
                            }
                            if (lessonDb.LessonRu != null)
                            {
                                return Conflict("This record already exists. Delete it or use PUT");
                            }
                            var newLessonRu = new LessonRu
                            {
                                Id = 0,
                                LessonId = lessonDb.Id,
                                Lesson = lessonDb,
                                Title = lessonPost.Title,
                            };
                            await _context.LessonsRu.AddAsync(newLessonRu);
                            break;
                        default:

                            if (lessonDb != null)
                            {
                                return Conflict("This name used.");
                            }

                            var newLesson = new Lesson
                            {
                                Id = 0,
                                Name = lessonPost.Name,
                                Title = lessonPost.Title,
                            };
                            await _context.Lessons.AddAsync(newLesson);
                            break;
                    }
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [EndpointSummary("Delete lessons by name list")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(int),
            Description = "Retruns number of deleted lessons or lessons + translations"
        )]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(
            [FromRoute] Languages language,
            [FromBody] string[] names
        )
        {
            try
            {
                foreach (var name in names)
                {
                    var enLesson = await _context
                        .Lessons.Include(l => l.LessonRu)
                        .FirstOrDefaultAsync(l => l.Name == name);
                    if (enLesson == null)
                    {
                        continue;
                    }
                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:

                            if (enLesson.LessonRu != null)
                            {
                                _context.LessonsRu.Remove(enLesson.LessonRu);
                            }

                            break;
                        default:

                            if (enLesson.LessonRu != null)
                            {
                                _context.LessonsRu.Remove(enLesson.LessonRu);
                            }
                            _context.Lessons.Remove(enLesson);
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
        [Route("/api/{language}/lesson")]
        [EndpointSummary("Find by name and refresh with provided title")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromRoute] Languages language,
            [FromBody] LessonPut lessonPut
        )
        {
            try
            {
                var enLesson = await _context
                    .Lessons.Include(l => l.LessonRu)
                    .FirstOrDefaultAsync(l => l.Name == lessonPut.Name);
                if (enLesson == null)
                {
                    return NotFound();
                }

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        if (enLesson.LessonRu == null)
                        {
                            return NotFound();
                        }
                        enLesson.LessonRu.Title = lessonPut.Title;
                        break;
                    default:

                        enLesson.Title = lessonPut.Title;
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
