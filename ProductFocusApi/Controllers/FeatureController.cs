using Common;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class FeatureController : ControllerBase
    {
        private readonly Messages _messages;

        public FeatureController(Messages messages)
        {
            _messages = messages;
        }


        [HttpPut]
        public async Task<IActionResult> ModifyFeatureElement([FromBody] UpdateFeatureDto dto)
        {
            var command = new UpdateFeatureCommand(dto);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{orgid}/{id}")]
        public async Task<IActionResult> GetFeatureDetailsById(long orgid, long id)
        {
            GetFeatureDetailsDto featureDetails = await _messages.Dispatch(new GetFeatureDetailsQuery(orgid, id));
            return Ok(featureDetails);
        }
    }
}
