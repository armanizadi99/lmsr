namespace Lmsr.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{

[HttpGet]
public async Task<IActionResult> GetTodos()
{
var todos = await _todoService.GetTodosAsync();
return Ok(todos);
}

[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateCourseCommand command)
{
var result = await _bus.Send(command);

if(!result.IsSuccess())
return BadRequest(result.Errors);

return CreatedAtAction(nameof(GetCourseById), new { id = result.Value);
}
}