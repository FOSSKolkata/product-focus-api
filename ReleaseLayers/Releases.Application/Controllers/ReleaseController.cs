using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Releases.Application.CommandHandler.ReleaseCommands;
using Releases.Application.QueryHandler;
using Releases.Domain.Model;

namespace Releases.Application.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    //[Authorize]
    public class ReleaseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ReleaseController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("{productId}")]
        public async Task<IActionResult> CreateRelease(long productId, AddReleaseDto dto)
        {
            var command = new AddReleaseCommand(productId, dto.Name, dto.ReleaseDate, dto.Status);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetReleasesByProductId(long productId)
        {
            var command = new GetReleasesQuery(productId);
            Result<List<GetReleaseDto>> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRelease([FromBody] UpdateReleaseDto dto)
        {
            var command = new UpdateReleaseCommand(dto.Id, dto.Name, dto.ReleaseDate, dto.Status);
            Result<Release> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
