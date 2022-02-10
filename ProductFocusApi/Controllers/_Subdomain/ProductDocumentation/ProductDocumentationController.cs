using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductDocumentations.Application.CommandHandlers.AddProductDocumentation;
using ProductDocumentations.Domain.Common;
using System.Threading.Tasks;

namespace ProductDocumentation.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    //[Authorize]
    public class ProductDocumentationController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductDocumentationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductDocumentation([FromBody] AddProductDocumentationDto dto)
        {
            var command = new AddProductDocumentationCommand(dto.ParentId, dto.ProductId, dto.Title, dto.Description);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
