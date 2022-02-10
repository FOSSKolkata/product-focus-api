using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using ProductFocus.Domain.Common;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class ModuleController : ControllerBase
    {
        private readonly Messages _messages;

        public ModuleController(Messages messages)
        {
            _messages = messages;
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> AddFeature(long id, [FromBody] AddFeatureDto dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var command = new AddFeatureCommand(id, dto.Title, dto.WorkItemType, dto.SprintId, objectId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
