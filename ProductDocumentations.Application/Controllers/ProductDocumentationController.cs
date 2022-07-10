using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductDocumentations.Application.CommandHandlers.AddProductDocumentation;
using ProductDocumentations.Application.CommandHandlers.DeleteProductDocumentation;
using ProductDocumentations.Application.CommandHandlers.UpdateProductDocumentation;
using ProductDocumentations.Application.QueryHandlers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductDocumentations.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductDocumentation(long id)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            Result result = await _mediator.Send(new DeleteProductDocumentationCommand(id, objectId));
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetFlatProductDocumentationsByProductId(long productId)
        {
            var command = new GetFlatProductDocumentationQuery(productId);
            Result<List<GetFlatProductDocumentationDto>> result = await _mediator.Send(command);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }
    }
}
