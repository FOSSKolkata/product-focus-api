using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocusApi.CommandHandlers;
using ProductFocusApi.Dtos;
using ProductFocusApi.QueryHandlers;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers
{
    [Route("[Controller]/[Action]")]
    [ApiController]
    [Authorize]
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
            var command = new AddTagCommand(productId,dto.Name, dto.TagCategoryId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetTagList(long productId)
        {
            List<GetTagDto> tagList = await _messages.Dispatch(new GetTagListQuery(productId));
            return Ok(tagList);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(long id)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new UpdateTagCommand(id, objectId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess? Ok(): BadRequest(result.Error);
        }
    }
}
