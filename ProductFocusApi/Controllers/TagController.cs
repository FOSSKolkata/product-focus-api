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
    [Route("[Controller]/[Action]")]
    [ApiController]
    public class TagController : Controller
    {
        private readonly Messages _messages;
        public TagController(Messages messages)
        {
            _messages = messages;
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddTag(long productId,[FromBody] AddTagDto dto)
        {
            var command = new AddTagCommand(productId,dto);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTagList(long productId)
        {
            List<GetTagDto> tagList = await _messages.Dispatch(new GetTagListQuery(productId));
            return Ok(tagList);
        }
    }
}
