using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TandemBackend.Data;
using TandemBackend.Models;

namespace TandemBackend.Controllers.Task
{
    [Route("api/{language}/tasks/code-editor")]
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
        [Route("/api/{language}/task/code-editor/{name}")]
        [EndpointSummary("Get code-editor task by name of lesson")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CodeEditorGetResult))]
        public async Task<IActionResult> GetCodeEditor(
            [FromRoute] Languages language,
            [FromRoute] string name
        )
        {
            try
            {
                CodeEditor? searchResult;
                CodeEditorGetResult result;

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .CodeEditor.Include(ce => ce.CodeEditorRu)
                                .ThenInclude(cer => cer!.Questions)
                            .FirstOrDefaultAsync(ce => ce.Name == name);

                        if (searchResult?.CodeEditorRu == null)
                        {
                            return NotFound();
                        }

                        var questionsRu = new List<CodeEditorQuestionGetResult>();

                        result = new CodeEditorGetResult
                        {
                            Id = searchResult.Id,
                            PracticeTopicId = searchResult.PracticeTopicId,
                            Title = searchResult.CodeEditorRu.Title,
                            Description = searchResult.CodeEditorRu.Description,
                            Questions = questionsRu,
                        };

                        foreach (var q in searchResult.CodeEditorRu.Questions)
                        {
                            questionsRu.Add(
                                new CodeEditorQuestionGetResult
                                {
                                    Id = q.Id,
                                    Title = q.Title,
                                    Description = q.Description,
                                    Instructions = q.Instructions,
                                    StarterCode = q.StarterCode,
                                    ExpectedAnswers = q.ExpectedAnswers,
                                    Hint = q.Hint,
                                }
                            );
                        }

                        break;
                    default:
                        searchResult = await _context
                            .CodeEditor.Include(cc => cc.Questions)
                            .FirstOrDefaultAsync(cc => cc.Name == name);

                        if (searchResult == null)
                        {
                            return NotFound();
                        }

                        var questions = new List<CodeEditorQuestionGetResult>();

                        result = new CodeEditorGetResult
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
                                new CodeEditorQuestionGetResult
                                {
                                    Id = q.Id,
                                    Title = q.Title,
                                    Description = q.Description,
                                    Instructions = q.Instructions,
                                    StarterCode = q.StarterCode,
                                    ExpectedAnswers = q.ExpectedAnswers,
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
        [EndpointSummary("Get all code-editor tasks")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CodeEditorGetResult>))]
        public async Task<IActionResult> GetAll([FromRoute] Languages language)
        {
            try
            {
                List<CodeEditor> searchResult;
                List<CodeEditorGetResult> result = new List<CodeEditorGetResult>();

                switch (language)
                {
                    case Languages.en:
                        goto default;
                    case Languages.ru:
                        searchResult = await _context
                            .CodeEditor.Include(cc => cc.CodeEditorRu)
                                .ThenInclude(ccr => ccr!.Questions)
                            .ToListAsync();
                        foreach (var sr in searchResult)
                        {
                            if (sr.CodeEditorRu == null)
                                continue;

                            var codeEditorQuestionGetResults =
                                new List<CodeEditorQuestionGetResult>();

                            var codeEditorGetResult = new CodeEditorGetResult
                            {
                                Id = sr.Id,
                                PracticeTopicId = sr.PracticeTopicId,
                                Title = sr.CodeEditorRu.Title,
                                Description = sr.CodeEditorRu.Description,
                                Questions = codeEditorQuestionGetResults,
                            };

                            foreach (var q in sr.CodeEditorRu.Questions)
                            {
                                codeEditorQuestionGetResults.Add(
                                    new CodeEditorQuestionGetResult
                                    {
                                        Id = q.Id,
                                        Title = q.Title,
                                        Description = q.Description,
                                        Instructions = q.Instructions,
                                        StarterCode = q.StarterCode,
                                        ExpectedAnswers = q.ExpectedAnswers,
                                        Hint = q.Hint,
                                    }
                                );
                            }
                            result.Add(codeEditorGetResult);
                        }

                        break;
                    default:
                        searchResult = await _context
                            .CodeEditor.Include(cc => cc.Questions)
                            .ToListAsync();
                        foreach (var sr in searchResult)
                        {
                            var codeEditorQuestionGetResults =
                                new List<CodeEditorQuestionGetResult>();

                            var codeEditorGetResult = new CodeEditorGetResult
                            {
                                Id = sr.Id,
                                PracticeTopicId = sr.PracticeTopicId,
                                Title = sr.Title,
                                Description = sr.Description,
                                Questions = codeEditorQuestionGetResults,
                            };

                            foreach (var q in sr.Questions)
                            {
                                codeEditorQuestionGetResults.Add(
                                    new CodeEditorQuestionGetResult
                                    {
                                        Id = q.Id,
                                        Title = q.Title,
                                        Description = q.Description,
                                        Instructions = q.Instructions,
                                        StarterCode = q.StarterCode,
                                        ExpectedAnswers = q.ExpectedAnswers,
                                        Hint = q.Hint,
                                    }
                                );
                            }
                            result.Add(codeEditorGetResult);
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
            [FromBody] CodeEditorPost[] codeEditorPosts
        )
        {
            try
            {
                foreach (var codeEditorPost in codeEditorPosts)
                {
                    var practiceTopicDb = await _context
                        .PracticeTopic.Where(pt => pt.Name == codeEditorPost.Name)
                        .Include(pt => pt.CodeEditor)
                            .ThenInclude(ce => ce!.CodeEditorRu)
                        .FirstOrDefaultAsync();

                    if (practiceTopicDb == null)
                    {
                        return NotFound(
                            $"CodeEditor with name \"{codeEditorPost.Name}\" not found, create CodeEditor first"
                        );
                    }

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:

                            if (practiceTopicDb.CodeEditor == null)
                            {
                                return NotFound("Create english version before other language");
                            }
                            if (practiceTopicDb.CodeEditor?.CodeEditorRu != null)
                            {
                                return Conflict("This record already exists. Delete it or use PUT");
                            }

                            var newQuestionsRu = new List<CodeEditorQuestionRu>();

                            var newCodeEditorRu = new CodeEditorRu
                            {
                                Id = 0,
                                CodeEditorId = practiceTopicDb.CodeEditor!.Id,
                                CodeEditor = practiceTopicDb.CodeEditor,
                                Title = codeEditorPost.Title,
                                Description = codeEditorPost.Description,
                                Questions = newQuestionsRu,
                            };

                            foreach (var q in codeEditorPost.Questions)
                            {
                                newQuestionsRu.Add(
                                    new CodeEditorQuestionRu
                                    {
                                        Id = 0,
                                        CodeEditorRuId = 0,
                                        CodeEditorRu = newCodeEditorRu,
                                        Title = q.Title,
                                        Description = q.Description,
                                        Instructions = q.Instructions,
                                        StarterCode = q.StarterCode,
                                        ExpectedAnswers = q.ExpectedAnswers,
                                        Hint = q.Hint ?? null,
                                    }
                                );
                            }

                            await _context.CodeEditorRu.AddAsync(newCodeEditorRu);

                            break;
                        default:

                            if (practiceTopicDb.CodeCompletion != null)
                            {
                                return Conflict("This name used. Delete it or use PUT");
                            }

                            var newQuestions = new List<CodeEditorQuestion>();

                            var newCodeEditor = new CodeEditor
                            {
                                Id = 0,
                                Name = codeEditorPost.Name,
                                PracticeTopicId = practiceTopicDb.Id,
                                PracticeTopic = practiceTopicDb,
                                Title = codeEditorPost.Title,
                                Description = codeEditorPost.Description,
                                Questions = newQuestions,
                            };

                            foreach (var q in codeEditorPost.Questions)
                            {
                                newQuestions.Add(
                                    new CodeEditorQuestion
                                    {
                                        Id = 0,
                                        CodeEditorId = 0,
                                        CodeEditor = newCodeEditor,
                                        Title = q.Title,
                                        Description = q.Description,
                                        Instructions = q.Instructions,
                                        StarterCode = q.StarterCode,
                                        ExpectedAnswers = q.ExpectedAnswers,
                                        Hint = q.Hint ?? null,
                                    }
                                );
                            }

                            await _context.CodeEditor.AddAsync(newCodeEditor);

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
        [EndpointSummary("Delete code-editor tasks by name list")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(int),
            Description = "Retruns number of deleted code-editor tasks by name"
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
                    CodeEditor? searchResult;

                    switch (language)
                    {
                        case Languages.en:
                            goto default;
                        case Languages.ru:
                            searchResult = await _context
                                .CodeEditor.Include(ce => ce.CodeEditorRu)
                                .FirstOrDefaultAsync(ce => ce.Name == name);

                            if (searchResult?.CodeEditorRu != null)
                            {
                                _context.CodeEditorRu.Remove(searchResult.CodeEditorRu);
                            }

                            break;
                        default:
                            searchResult = await _context.CodeEditor.FirstOrDefaultAsync(ce =>
                                ce.Name == name
                            );
                            if (searchResult == null)
                                continue;

                            _context.CodeEditor.Remove(searchResult!);

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
