using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
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

        [HttpGet]
        public async Task<IActionResult> GetOrganizationListByUser()
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;                      

            List<GetOrganizationByUserDto> organizationList = await _messages.Dispatch(new GetOrganizationListByUserQuery(objectId));
            return Ok(organizationList);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationDto dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new AddOrganizationCommand(dto.OrganizationName, objectId);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> AddProduct(long id, [FromBody] AddProductDto dto)
        {
            var command = new AddProductCommand(id, dto.Name);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductsById(long id)
        {
            List<GetProductDto> organizationList = await _messages.Dispatch(new GetProductListQuery(id));
            return Ok(organizationList);
        }
    }
}
