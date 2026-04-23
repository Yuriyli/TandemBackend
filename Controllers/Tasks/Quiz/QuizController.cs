using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Task
{
    [Route("api/{language}/tasks/quiz")]
    [ApiController]
    [Tags("Tasks")]
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
        [Route("/api/{language}/task/quiz/{name}")]
        [EndpointSummary("Get quiz by name of lesson")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuizGetResult))]
        public async Task<IActionResult> GetQuiz(
            [FromRoute] Languages language,
            [FromRoute] string name
        )
        {
            try
            {
                Quiz? searchResult;
                QuizGetResult result;

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .Quiz.Include(q => q.QuizRu)
                                .ThenInclude(qr => qr!.Questions)
                            .FirstOrDefaultAsync(q => q.Name == name);

                        if (searchResult?.QuizRu == null)
                        {
                            return NotFound();
                        }

                        var getQuestionsRu = new List<QuizQuestionGetResult>();

                        result = new QuizGetResult
                        {
                            Id = searchResult.Id,
                            PracticeTopicId = searchResult.PracticeTopicId,
                            Title = searchResult.QuizRu.Title,
                            Description = searchResult.QuizRu.Description,
                            Questions = getQuestionsRu,
                        };

                        foreach (var q in searchResult.QuizRu.Questions)
                        {
                            getQuestionsRu.Add(
                                new QuizQuestionGetResult
                                {
                                    Id = q.Id,
                                    Question = q.Question,
                                    CorrectAnswer = q.CorrectAnswer,
                                    Options = q.Options,
                                }
                            );
                        }

                        break;
                    default:
                        searchResult = await _context
                            .Quiz.Include(q => q.Questions)
                            .FirstOrDefaultAsync(q => q.Name == name);
                        if (searchResult == null)
                        {
                            return NotFound();
                        }

                        var getQuestions = new List<QuizQuestionGetResult>();

                        result = new QuizGetResult
                        {
                            Id = searchResult.Id,
                            PracticeTopicId = searchResult.PracticeTopicId,
                            Title = searchResult.Title,
                            Description = searchResult.Description,
                            Questions = getQuestions,
                        };

                        foreach (var q in searchResult.Questions)
                        {
                            getQuestions.Add(
                                new QuizQuestionGetResult
                                {
                                    Id = q.Id,
                                    Question = q.Question,
                                    CorrectAnswer = q.CorrectAnswer,
                                    Options = q.Options,
                                }
                            );
                        }

                        break;
                }

                return Ok(result);
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.StackTrace);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [EndpointSummary("Get all quizzes ")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<QuizGetResult>))]
        public async Task<IActionResult> GetAllQuizzes([FromRoute] Languages language)
        {
            try
            {
                List<Quiz> searchResult;
                List<QuizGetResult> result = new List<QuizGetResult>();

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .Quiz.Include(pt => pt.QuizRu)
                                .ThenInclude(qr => qr!.Questions)
                            .ToListAsync();
                        foreach (var sr in searchResult)
                        {
                            if (sr.QuizRu == null)
                                continue;

                            var quizQuestionGetResults = new List<QuizQuestionGetResult>();

                            var quizGetResult = new QuizGetResult
                            {
                                Id = sr.Id,
                                PracticeTopicId = sr.PracticeTopicId,
                                Title = sr.QuizRu.Title,
                                Description = sr.QuizRu.Description,
                                Questions = quizQuestionGetResults,
                            };

                            foreach (var q in sr.QuizRu.Questions)
                            {
                                quizQuestionGetResults.Add(
                                    new QuizQuestionGetResult
                                    {
                                        Id = q.Id,
                                        Question = q.Question,
                                        CorrectAnswer = q.CorrectAnswer,
                                        Options = q.Options,
                                    }
                                );
                            }
                            result.Add(quizGetResult);
                        }

                        break;
                    default:
                        searchResult = await _context
                            .Quiz.Include(pt => pt.Questions)
                            .ToListAsync();
                        foreach (var sr in searchResult)
                        {
                            var quizQuestionGetResults = new List<QuizQuestionGetResult>();

                            var quizGetResult = new QuizGetResult
                            {
                                Id = sr.Id,
                                PracticeTopicId = sr.PracticeTopicId,
                                Title = sr.Title,
                                Description = sr.Description,
                                Questions = quizQuestionGetResults,
                            };

                            foreach (var q in sr.Questions)
                            {
                                quizQuestionGetResults.Add(
                                    new QuizQuestionGetResult
                                    {
                                        Id = q.Id,
                                        Question = q.Question,
                                        CorrectAnswer = q.CorrectAnswer,
                                        Options = q.Options,
                                    }
                                );
                            }
                            result.Add(quizGetResult);
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
        [EndpointSummary("Create new quizzes by array of new quizzes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(
            StatusCodes.Status409Conflict,
            Description = "This name used. Name field should be unique for all lenguages. Try DELETE or PUT."
        )]
        public async Task<IActionResult> Post(
            [FromRoute] Languages language,
            [FromBody] QuizPost[] quizPosts
        )
        {
            try
            {
                foreach (var quizPost in quizPosts)
                {
                    var practiceTopicDb = await _context
                        .PracticeTopic.Where(pt => pt.Name == quizPost.Name)
                        .Include(pt => pt.Quiz)
                            .ThenInclude(q => q!.QuizRu)
                        .FirstOrDefaultAsync();

                    if (practiceTopicDb == null)
                    {
                        return NotFound(
                            $"PracticeTopic with name \"{quizPost.Name}\" not found, create PracticeTopic first"
                        );
                    }

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:

                            if (practiceTopicDb.Quiz == null)
                            {
                                return NotFound("Create english version before other language");
                            }
                            if (practiceTopicDb.Quiz?.QuizRu != null)
                            {
                                return Conflict("This record already exists. Delete it or use PUT");
                            }

                            var newQuestionsRu = new List<QuizQuestionRu>();

                            var newQuizRu = new QuizRu
                            {
                                Id = 0,
                                QuizId = practiceTopicDb.Quiz!.Id,
                                Quiz = practiceTopicDb.Quiz,
                                Title = quizPost.Title,
                                Description = quizPost.Description,
                                Questions = newQuestionsRu,
                            };

                            foreach (var q in quizPost.Questions)
                            {
                                newQuestionsRu.Add(
                                    new QuizQuestionRu
                                    {
                                        Id = 0,
                                        QuizRuId = 0,
                                        QuizRu = newQuizRu,
                                        Question = q.Question,
                                        CorrectAnswer = q.CorrectAnswer,
                                        Options = q.Options,
                                    }
                                );
                            }

                            await _context.QuizRu.AddAsync(newQuizRu);

                            break;
                        default:

                            if (practiceTopicDb.Quiz != null)
                            {
                                return Conflict("This name used. Delete it or use PUT");
                            }

                            var newQuestions = new List<QuizQuestion>();

                            var newQuiz = new Quiz
                            {
                                Id = 0,
                                Name = quizPost.Name,
                                PracticeTopicId = practiceTopicDb.Id,
                                PracticeTopic = practiceTopicDb,
                                Title = quizPost.Title,
                                Description = quizPost.Description,
                                Questions = newQuestions,
                            };
                            foreach (var q in quizPost.Questions)
                            {
                                newQuestions.Add(
                                    new QuizQuestion
                                    {
                                        Id = 0,
                                        QuizId = 0,
                                        Quiz = newQuiz,
                                        Question = q.Question,
                                        CorrectAnswer = q.CorrectAnswer,
                                        Options = q.Options,
                                    }
                                );
                            }

                            await _context.Quiz.AddAsync(newQuiz);

                            break;
                    }
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        [EndpointSummary("Delete quizzes by name list")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(int),
            Description = "Retruns number of deleted quizzes by name"
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
                    Quiz? searchResult;

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:
                            searchResult = await _context
                                .Quiz.Include(q => q.QuizRu)
                                .FirstOrDefaultAsync(q => q.Name == name);

                            if (searchResult?.QuizRu != null)
                            {
                                _context.QuizRu.Remove(searchResult.QuizRu);
                            }

                            break;
                        default:
                            searchResult = await _context.Quiz.FirstOrDefaultAsync(q =>
                                q.Name == name
                            );
                            if (searchResult == null)
                                continue;

                            _context.Quiz.Remove(searchResult!);

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
    }
}
