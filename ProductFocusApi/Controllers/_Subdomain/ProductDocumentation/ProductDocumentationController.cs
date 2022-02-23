using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductDocumentations.Application.CommandHandlers.AddProductDocumentation;
using ProductDocumentations.Application.CommandHandlers.UpdateProductDocumentation;
using ProductDocumentations.Application.QueryHandlers;
using System.Collections.Generic;
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

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductDocumentations(long productId)
        {
            List<GetProductDocumentationDto> productDocumentations = await _mediator.Send(new GetProductDocumentationsQuery(productId));
            return Ok(productDocumentations);
        }

        [HttpGet("{id}/{index}")]
        public async Task<IActionResult> GetProductDocumentationById(long id, int index)
        {
            List<GetProductDocumentationDetailsDto> productDocumentationsDetail = await _mediator.Send(new GetProductDocumentationQuery(id, index));
            return Ok(productDocumentationsDetail);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductDocumentation(UpdateProductDocumentationCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductDocumentationOrdering([FromBody] UpdateProductDocumentationOrderingCommand command)
        {
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
