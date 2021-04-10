using Common;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.Persistence.AppServices;
using ProductFocus.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ProductFocus.Persistence.Common;

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

            return Ok("Test message from product focus");
        }

        [HttpPost]
        public IActionResult AddOrganization([FromBody] OrganizationDto dto)
        {
            var command = new AddOrganizationCommand(dto.Name);
            Result result = _messages.Dispatch(command);
            return Ok();
        }
    }
}
