using Common;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using ProductFocus.Domain;

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
        public IActionResult AddOrganization([FromBody] AddOrganizationDto dto)
        {
            var command = new AddOrganizationCommand(dto.Name);
            Result result = _messages.Dispatch(command);
            return Ok();
        }
    }
}
