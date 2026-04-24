using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Task
{
    [Route("api/{language}/tasks/code-completion")]
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
        [Route("/api/{language}/task/code-completion/{name}")]
        [EndpointSummary("Get code-completion by name of lesson")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CodeCompletionGetResult))]
        public async Task<IActionResult> GetCodeCompletion(
            [FromRoute] Languages language,
            [FromRoute] string name
        )
        {
            try
            {
                CodeCompletion? searchResult;
                CodeCompletionGetResult result;

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .CodeCompletion.Include(cc => cc.CodeCompletionRu)
                                .ThenInclude(ccr => ccr!.Questions)
                            .FirstOrDefaultAsync(cc => cc.Name == name);

                        if (searchResult?.CodeCompletionRu == null)
                        {
                            return NotFound();
                        }

                        var questionsRu = new List<CodeCompletionQuestionGetResult>();

                        result = new CodeCompletionGetResult
                        {
                            Id = searchResult.Id,
                            PracticeTopicId = searchResult.PracticeTopicId,
                            Title = searchResult.CodeCompletionRu.Title,
                            Description = searchResult.CodeCompletionRu.Description,
                            Questions = questionsRu,
                        };

                        foreach (var q in searchResult.CodeCompletionRu.Questions)
                        {
                            questionsRu.Add(
                                new CodeCompletionQuestionGetResult
                                {
                                    Id = q.Id,
                                    Title = q.Title,
                                    Description = q.Description,
                                    Code = q.Code,
                                    CorrectAnswer = q.CorrectAnswer,
                                    Options = q.Options,
                                    Hint = q.Hint,
                                }
                            );
                        }

                        break;
                    default:
                        searchResult = await _context
                            .CodeCompletion.Include(cc => cc.Questions)
                            .FirstOrDefaultAsync(cc => cc.Name == name);

                        if (searchResult == null)
                        {
                            return NotFound();
                        }

                        var questions = new List<CodeCompletionQuestionGetResult>();

                        result = new CodeCompletionGetResult
                        {
                            Id = searchResult.Id,
                            PracticeTopicId = searchResult.PracticeTopicId,
                            Title = searchResult.Title,
                            Description = searchResult.Description,
                            Questions = questions,
                        };

                        foreach (var q in searchResult.Questions)
                        {
                            questions.Add(
                                new CodeCompletionQuestionGetResult
                                {
                                    Id = q.Id,
                                    Title = q.Title,
                                    Description = q.Description,
                                    Code = q.Code,
                                    CorrectAnswer = q.CorrectAnswer,
                                    Options = q.Options,
                                    Hint = q.Hint,
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

        [HttpGet]
        [EndpointSummary("Get all code-completion tasks")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(List<CodeCompletionGetResult>)
        )]
        public async Task<IActionResult> GetAll([FromRoute] Languages language)
        {
            try
            {
                List<CodeCompletion> searchResult;
                List<CodeCompletionGetResult> result = new List<CodeCompletionGetResult>();

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .CodeCompletion.Include(cc => cc.CodeCompletionRu)
                                .ThenInclude(ccr => ccr!.Questions)
                            .ToListAsync();
                        foreach (var sr in searchResult)
                        {
                            if (sr.CodeCompletionRu == null)
                                continue;

                            var codeCompletionQuestionGetResults =
                                new List<CodeCompletionQuestionGetResult>();

                            var codeCompletionGetResult = new CodeCompletionGetResult
                            {
                                Id = sr.Id,
                                PracticeTopicId = sr.PracticeTopicId,
                                Title = sr.CodeCompletionRu.Title,
                                Description = sr.CodeCompletionRu.Description,
                                Questions = codeCompletionQuestionGetResults,
                            };

                            foreach (var q in sr.CodeCompletionRu.Questions)
                            {
                                codeCompletionQuestionGetResults.Add(
                                    new CodeCompletionQuestionGetResult
                                    {
                                        Id = q.Id,
                                        Title = q.Title,
                                        Description = q.Description,
                                        Code = q.Code,
                                        CorrectAnswer = q.CorrectAnswer,
                                        Options = q.Options,
                                    }
                                );
                            }
                            result.Add(codeCompletionGetResult);
                        }

                        break;
                    default:
                        searchResult = await _context
                            .CodeCompletion.Include(cc => cc.Questions)
                            .ToListAsync();
                        foreach (var sr in searchResult)
                        {
                            var codeCompletionQuestionGetResults =
                                new List<CodeCompletionQuestionGetResult>();

                            var codeCompletionGetResult = new CodeCompletionGetResult
                            {
                                Id = sr.Id,
                                PracticeTopicId = sr.PracticeTopicId,
                                Title = sr.Title,
                                Description = sr.Description,
                                Questions = codeCompletionQuestionGetResults,
                            };

                            foreach (var q in sr.Questions)
                            {
                                codeCompletionQuestionGetResults.Add(
                                    new CodeCompletionQuestionGetResult
                                    {
                                        Id = q.Id,
                                        Title = q.Title,
                                        Description = q.Description,
                                        Code = q.Code,
                                        CorrectAnswer = q.CorrectAnswer,
                                        Options = q.Options,
                                    }
                                );
                            }
                            result.Add(codeCompletionGetResult);
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
        [EndpointSummary("Create new code-completion tsk by array of new tasks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(
            StatusCodes.Status409Conflict,
            Description = "This name used. Name field should be unique for all lenguages. Try DELETE or PUT."
        )]
        public async Task<IActionResult> Post(
            [FromRoute] Languages language,
            [FromBody] CodeCompletionPost[] codeCompletionPosts
        )
        {
            try
            {
                foreach (var codeCompletionPost in codeCompletionPosts)
                {
                    var practiceTopicDb = await _context
                        .PracticeTopic.Where(pt => pt.Name == codeCompletionPost.Name)
                        .Include(pt => pt.CodeCompletion)
                            .ThenInclude(cq => cq!.CodeCompletionRu)
                        .FirstOrDefaultAsync();

                    if (practiceTopicDb == null)
                    {
                        return NotFound(
                            $"PracticeTopic with name \"{codeCompletionPost.Name}\" not found, create PracticeTopic first"
                        );
                    }

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:

                            if (practiceTopicDb.CodeCompletion == null)
                            {
                                return NotFound("Create english version before other language");
                            }
                            if (practiceTopicDb.CodeCompletion?.CodeCompletionRu != null)
                            {
                                return Conflict("This record already exists. Delete it or use PUT");
                            }

                            var newQuestionsRu = new List<CodeCompletionQuestionRu>();

                            var newCodeCompletionRu = new CodeCompletionRu
                            {
                                Id = 0,
                                CodeCompletionId = practiceTopicDb.CodeCompletion!.Id,
                                CodeCompletion = practiceTopicDb.CodeCompletion,
                                Title = codeCompletionPost.Title,
                                Description = codeCompletionPost.Description,
                                Questions = newQuestionsRu,
                            };

                            foreach (var q in codeCompletionPost.Questions)
                            {
                                newQuestionsRu.Add(
                                    new CodeCompletionQuestionRu
                                    {
                                        Id = 0,
                                        CodeCompletionRuId = 0,
                                        CodeCompletionRu = newCodeCompletionRu,
                                        Title = q.Title,
                                        Description = q.Description,
                                        Code = q.Code,
                                        CorrectAnswer = q.CorrectAnswer,
                                        Hint = q.Hint ?? null,
                                        Options = q.Options,
                                    }
                                );
                            }

                            await _context.CodeCompletionRu.AddAsync(newCodeCompletionRu);

                            break;
                        default:

                            if (practiceTopicDb.CodeCompletion != null)
                            {
                                return Conflict("This name used. Delete it or use PUT");
                            }

                            var newQuestions = new List<CodeCompletionQuestion>();

                            var newCodeCompletion = new CodeCompletion
                            {
                                Id = 0,
                                Name = codeCompletionPost.Name,
                                PracticeTopicId = practiceTopicDb.Id,
                                PracticeTopic = practiceTopicDb,
                                Title = codeCompletionPost.Title,
                                Description = codeCompletionPost.Description,
                                Questions = newQuestions,
                            };

                            foreach (var q in codeCompletionPost.Questions)
                            {
                                newQuestions.Add(
                                    new CodeCompletionQuestion
                                    {
                                        Id = 0,
                                        CodeCompletionId = 0,
                                        CodeCompletion = newCodeCompletion,
                                        Title = q.Title,
                                        Description = q.Description,
                                        Code = q.Code,
                                        CorrectAnswer = q.CorrectAnswer,
                                        Hint = q.Hint ?? null,
                                        Options = q.Options,
                                    }
                                );
                            }

                            await _context.CodeCompletion.AddAsync(newCodeCompletion);

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
        [EndpointSummary("Delete code-completion tasks by name list")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(int),
            Description = "Retruns number of deleted code-completion tasks by name"
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
                    CodeCompletion? searchResult;

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:
                            searchResult = await _context
                                .CodeCompletion.Include(cc => cc.CodeCompletionRu)
                                .FirstOrDefaultAsync(cc => cc.Name == name);

                            if (searchResult?.CodeCompletionRu != null)
                            {
                                _context.CodeCompletionRu.Remove(searchResult.CodeCompletionRu);
                            }

                            break;
                        default:
                            searchResult = await _context.CodeCompletion.FirstOrDefaultAsync(cc =>
                                cc.Name == name
                            );
                            if (searchResult == null)
                                continue;

                            _context.CodeCompletion.Remove(searchResult!);

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
