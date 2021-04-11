using Common;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class OrganizationController : ControllerBase
    {
        private readonly Messages _messages;

        public OrganizationController(Messages messages)
        {
            _messages = messages;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganizationList()
        {
            List<GetOrganizationDto> organizationList = await _messages.Dispatch(new GetOrganizationListQuery());
            return Ok(organizationList);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationDto dto)
        {
            var command = new AddOrganizationCommand(dto.Name);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : NotFound();
        }
    }
}
