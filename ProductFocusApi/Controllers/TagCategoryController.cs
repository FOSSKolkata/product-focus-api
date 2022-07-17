using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.Domain.Common;
using ProductFocusApi.CommandHandlers;
using ProductFocusApi.Dtos;
using ProductFocusApi.QueryHandlers;
using System.Collections.Generic;
using System.Threading.Tasks;
//Tagmanagement
namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class TagCategoryController : Controller
    {
        private readonly IMediator _mediator;
        public TagCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("{productId}")]
        public async Task<IActionResult> AddTagCategory(long productId, [FromBody] AddTagCategoryDto dto)
        {
            var command = new AddTagCategoryCommand(productId, dto.Name);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTagCategories(long productId)
        {
            List<GetTagCategoryDto> tagCategoryList = await _mediator.Send(new GetTagCategoryListQuery(productId));
            return Ok(tagCategoryList);
        }
    }
}
