using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Lmsr.Domain.Aggregates;
using Lmsr.Application.Courses;
using Lmsr.Presentation.Dtos;
using System.Security.Claims;
namespace Lmsr.Presentation.Controllers;
[Authorize]
[ApiController]
[Route("api/Courses/[controller]")]
public class WordController : ControllerBase
{
private IMediator _bus;
public WordController(IMediator bus)
{
_bus = bus;
}
[HttpPost]
public async Task<IActionResult> AddWordToCourse([FromBody] AddWordToCourseDto dto)
{
var result = await _bus.Send(new AddWordToCourseCommand(dto.Term, dto.CourseId));
if(!result.IsSuccess)
return BadRequest(result.Error);
return CreatedAtAction(nameof(GetWordById), new {Id = result.Value.Id}, result.Value);
}
[HttpGet]
public async Task<IActionResult> GetWordById([FromQuery] int Id)
{
var result = await _bus.Send(new GetCourseWordByIdQuery(Id));
if(!result.IsSuccess)
return BadRequest(result.Error);
return Ok(result.Value);
}[Route("for")]
[HttpGet]
[AllowAnonymous]
public async Task<IActionResult> GetAllWordsForCourse([FromQuery] int courseId)
{
var result = await _bus.Send(new GetCourseWordsQuery(courseId));
return Ok(result.Value);
}
}