using Common;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;
using System.Collections.Generic;

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
        public IActionResult GetOrganizationList()
        {
            List<GetOrganizationDto> organizationList = _messages.Dispatch(new GetOrganizationListQuery());
            return Ok(organizationList);
        }

        [HttpPost]
        public IActionResult AddOrganization([FromBody] AddOrganizationDto dto)
        {
            var command = new AddOrganizationCommand(dto.Name);
            Result result = _messages.Dispatch(command);
            return Ok();
        }
    }
}
