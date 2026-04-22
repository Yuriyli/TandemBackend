using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Lessons
{
    [Route("api/{language}/topics")]
    [ApiController]
    [Tags("Topics")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public TasksController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("/api/{language}/topic/{name}")]
        [EndpointSummary("Get practice topic by name of lesson")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PracticeTopicGetResult))]
        public async Task<IActionResult> GetTopic(
            [FromRoute] Languages language,
            [FromRoute] string name
        )
        {
            try
            {
                PracticeTopic? searchResult;
                PracticeTopicGetResult result;

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .PracticeTopic.Include(pt => pt.PracticeTopicRu)
                            .FirstOrDefaultAsync(pt => pt.Name == name);

                        if (searchResult?.PracticeTopicRu == null)
                        {
                            return NotFound();
                        }

                        result = new PracticeTopicGetResult
                        {
                            Id = searchResult.Id,
                            LessonId = searchResult.LessonId,
                            Name = searchResult.Name,
                            Title = searchResult.PracticeTopicRu.Title,
                            Description = searchResult.PracticeTopicRu.Description,
                        };

                        break;
                    default:
                        searchResult = await _context
                            .PracticeTopic.Include(pt => pt.PracticeTopicRu)
                            .FirstOrDefaultAsync(pt => pt.Name == name);
                        if (searchResult == null)
                        {
                            return NotFound();
                        }

                        result = new PracticeTopicGetResult
                        {
                            Id = searchResult.Id,
                            LessonId = searchResult.LessonId,
                            Name = searchResult.Name,
                            Title = searchResult.Title,
                            Description = searchResult.Description,
                        };

                        break;
                }

                return Ok(result);
            }
            catch (System.Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [EndpointSummary("Get practice topics list")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PracticeTopicGetResult>))]
        public async Task<IActionResult> GetTopics([FromRoute] Languages language)
        {
            try
            {
                List<PracticeTopic> searchResult;
                List<PracticeTopicGetResult> result = new List<PracticeTopicGetResult>();

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .PracticeTopic.Include(pt => pt.PracticeTopicRu)
                            .ToListAsync();
                        foreach (var sr in searchResult)
                        {
                            if (sr.PracticeTopicRu == null)
                                continue;
                            result.Add(
                                new PracticeTopicGetResult
                                {
                                    Id = sr.Id,
                                    LessonId = sr.LessonId,
                                    Name = sr.Name,
                                    Title = sr.PracticeTopicRu.Title,
                                    Description = sr.PracticeTopicRu.Description,
                                }
                            );
                        }

                        break;
                    default:
                        searchResult = await _context.PracticeTopic.ToListAsync();

                        foreach (var sr in searchResult)
                        {
                            result.Add(
                                new PracticeTopicGetResult
                                {
                                    Id = sr.Id,
                                    LessonId = sr.LessonId,
                                    Name = sr.Name,
                                    Title = sr.Title,
                                    Description = sr.Description,
                                }
                            );
                        }

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
        [EndpointSummary("Create new practice topics by array of new topics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(
            StatusCodes.Status409Conflict,
            Description = "This name used. Name field should be unique for all lenguages. Try DELETE or PUT."
        )]
        public async Task<IActionResult> Post(
            [FromRoute] Languages language,
            [FromBody] PracticeTopicPost[] practiceTopicPosts
        )
        {
            try
            {
                foreach (var practiceTopicPost in practiceTopicPosts)
                {
                    var lessonDb = await _context
                        .Lessons.Where(l => l.Name == practiceTopicPost.Name)
                        .Include(l => l.PracticeTopic)
                            .ThenInclude(pt => pt!.PracticeTopicRu)
                        .FirstOrDefaultAsync();

                    if (lessonDb == null)
                    {
                        return NotFound(
                            $"Lesson with name \"{practiceTopicPost.Name}\" not found, create lesson first"
                        );
                    }

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:

                            if (lessonDb.PracticeTopic == null)
                            {
                                return NotFound("Create english version before other language");
                            }
                            if (lessonDb.PracticeTopic?.PracticeTopicRu != null)
                            {
                                return Conflict("This record already exists. Delete it or use PUT");
                            }

                            var newPracticeTopicRu = new PracticeTopicRu
                            {
                                Id = 0,
                                PracticeTopicId = lessonDb.PracticeTopic!.Id,
                                PracticeTopic = lessonDb.PracticeTopic,
                                Title = practiceTopicPost.Title,
                                Description = practiceTopicPost.Description,
                            };
                            await _context.PracticeTopicRu.AddAsync(newPracticeTopicRu);

                            break;
                        default:

                            if (lessonDb.PracticeTopic != null)
                            {
                                return Conflict("This name used.");
                            }

                            var newPracticeTopic = new PracticeTopic
                            {
                                Id = 0,
                                LessonId = lessonDb.Id,
                                Lesson = lessonDb,
                                Name = practiceTopicPost.Name,
                                Title = practiceTopicPost.Title,
                                Description = practiceTopicPost.Description,
                            };
                            await _context.PracticeTopic.AddAsync(newPracticeTopic);

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
        [EndpointSummary("Delete practice topics by name list")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(int),
            Description = "Retruns number of deleted topics or topics + translations"
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
                    PracticeTopic? searchResult;

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:
                            searchResult = await _context
                                .PracticeTopic.Include(pt => pt.PracticeTopicRu)
                                .FirstOrDefaultAsync(pt => pt.Name == name);

                            if (searchResult?.PracticeTopicRu != null)
                            {
                                _context.PracticeTopicRu.Remove(searchResult.PracticeTopicRu);
                            }

                            break;
                        default:
                            searchResult = await _context.PracticeTopic.FirstOrDefaultAsync(pt =>
                                pt.Name == name
                            );
                            if (searchResult == null)
                                continue;

                            if (searchResult?.PracticeTopicRu != null)
                            {
                                _context.PracticeTopicRu.Remove(searchResult.PracticeTopicRu);
                            }

                            _context.PracticeTopic.Remove(searchResult!);

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
        [Route("/api/{language}/topic")]
        [EndpointSummary("Find by name and refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(
            [FromRoute] Languages language,
            [FromBody] PracticeTopicPut practiceTopicPut
        )
        {
            Console.WriteLine(practiceTopicPut);
            try
            {
                PracticeTopic? searchResult;

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .PracticeTopic.Include(pt => pt.PracticeTopicRu)
                            .FirstOrDefaultAsync(pt => pt.Name == practiceTopicPut.Name);

                        if (searchResult?.PracticeTopicRu == null)
                        {
                            return NotFound();
                        }

                        searchResult.PracticeTopicRu.Title = practiceTopicPut.Title;
                        searchResult.PracticeTopicRu.Description = practiceTopicPut.Description;

                        break;
                    default:
                        searchResult = await _context.PracticeTopic.FirstOrDefaultAsync(pt =>
                            pt.Name == practiceTopicPut.Name
                        );

                        if (searchResult == null)
                        {
                            return NotFound();
                        }

                        searchResult.Title = practiceTopicPut.Title;
                        searchResult.Description = practiceTopicPut.Description;

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
