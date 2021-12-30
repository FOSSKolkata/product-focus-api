using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocusApi.CommandHandlers;
using ProductFocusApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    //[Authorize]
    public class BusinessRequirementController : ControllerBase
    {
        private readonly Messages _messages;
        public BusinessRequirementController(Messages messages)
        {
            _messages = messages;
        }
        [HttpPost]
        public async Task<IActionResult> AddBusinessRequirement([FromBody] AddBusinessRequirementDto dto)
        {
            var command = new AddBusinessRequirementCommand(dto.Date, dto.TagIds, dto.Source, dto.SourceAdditionalInformation, dto.Description, dto.Files);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
