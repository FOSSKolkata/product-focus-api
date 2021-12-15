using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocusApi.CommandHandlers;
using ProductFocusApi.Dtos;
using ProductFocusApi.QueryHandlers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class TagCategoryController : Controller
    {
        private readonly Messages _messages;
        public TagCategoryController(Messages messages)
        {
            _messages = messages;
        }
        [HttpPost("{productId}")]
        public async Task<IActionResult> AddTagCategory(long productId, [FromBody] string name)
        {
            var command = new AddTagCategoryCommand(productId, name);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTagCategory(long productId)
        {
            List<GetTagCategoryDto> tagCategoryList = await _messages.Dispatch(new GetTagCategoryListQuery(productId));
            return Ok(tagCategoryList);
        }
    }
}
