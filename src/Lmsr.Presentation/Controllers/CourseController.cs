using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Lmsr.Domain.Aggregates;
using Lmsr.Application.Courses;

namespace Lmsr.Presentation.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
private IMediator _bus;
public CourseController(IMediator bus)
{
_bus = bus;
}
[HttpPost]
public async Task<IActionResult> Create([FromBody] CreateCourseDto dto)
{
var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
var result = await _bus.Send(new CreateCourseCommand(dto.Title, userId, false));

if(!result.IsSuccess)
return BadRequest(result.Errors);

return CreatedAtAction(nameof(GetCourseById), new { id = result.Value});
}

[HttpGet]
[AllowAnonymous]
public async Task<IActionResult> GetCourseById([FromQuery] int id)
{
var result = await _bus.Send(new GetAllCoursesQuery());
if(!result.IsSuccess)
return BadRequest(result.Errors);
var course = result.Value.FirstOrDefault(c => c.Id == id);
if(course == null)
return NotFound();
return Ok(course);
}

[Route("all")]
[HttpGet]
[AllowAnonymous]
public async Task<IActionResult> GetAllCourses()
{
return Ok((await _bus.Send(new GetAllCoursesQuery())).Value);
}
}