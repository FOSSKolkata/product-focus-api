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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrganizationListByUser(long id)
        {
            List<GetOrganizationByUserDto> organizationList = await _messages.Dispatch(new GetOrganizationListByUserQuery(id));
            return Ok(organizationList);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationDto dto)
        {
            var command = new AddOrganizationCommand(dto.OrganizationName, dto.Email);
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
