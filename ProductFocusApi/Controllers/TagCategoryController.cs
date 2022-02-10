using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.Domain.Common;
using ProductFocusApi.CommandHandlers;
using ProductFocusApi.Dtos;
using ProductFocusApi.QueryHandlers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class TagCategoryController : Controller
    {
        private readonly Messages _messages;
        public TagCategoryController(Messages messages)
        {
            _messages = messages;
        }
        [HttpPost("{productId}")]
        public async Task<IActionResult> AddTagCategory(long productId, [FromBody] AddTagCategoryDto dto)
        {
            var command = new AddTagCategoryCommand(productId, dto.Name);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTagCategories(long productId)
        {
            List<GetTagCategoryDto> tagCategoryList = await _messages.Dispatch(new GetTagCategoryListQuery(productId));
            return Ok(tagCategoryList);
        }
    }
}
