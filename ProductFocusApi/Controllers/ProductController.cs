using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using ProductFocus.Domain.Common;
using MediatR;
using ProductFocus.Domain.Model;
using ProductFocusApi.QueryHandlers;
using ProductFocusApi.Dtos;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKanbanViewByProductId(long id)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            List<GetKanbanViewDto> kanbanViewList = await _mediator.Send(new GetKanbanViewQuery(id, objectId));
            return Ok(kanbanViewList);
        }

        [HttpGet("{id}/{groupCategoryEnum}/query")]
        public async Task<IActionResult> GetKanbanViewByProductIdAndQuery(long id, [FromQuery] long? SprintId, [FromQuery] List<long> UserIds, GroupCategoryEnum groupCategoryEnum)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            GetKanbanViewListDto kanban = await _mediator.Send(new GetKanbanViewFilterQuery(id, objectId, SprintId, UserIds, groupCategoryEnum));
            return Ok(kanban);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(long id)
        {
            GetProductByIdDto product = await _mediator.Send(new GetProductByIdQuery(id)); 
            return Ok(product);
        }
        [HttpPost("{id}")]
        public async Task<IActionResult> AddProduct(long id, [FromBody] AddProductDto dto)
        {
            var command = new AddProductCommand(id, dto.Name);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsById(long id)
        {
            List<GetProductDto> organizationList = await _mediator.Send(new GetProductListQuery(id));
            return Ok(organizationList);
        }
    }
}
