using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using ProductFocus.Domain.Common;
using ProductFocusApi.QueryHandlers;
using MediatR;
using ProductFocusApi.Dtos;
//TenentManagement
namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class OrganizationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrganizationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganizationList()
        {
           
            List<GetOrganizationDto> organizationList = await _mediator.Send(new GetOrganizationListQuery());
            return Ok(organizationList);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrganizationListByUser()
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;                      

            List<GetOrganizationByUserDto> organizationList = await _mediator.Send(new GetOrganizationListByUserQuery(objectId));
            return Ok(organizationList);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrganization([FromBody] AddOrganizationDto dto)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new AddOrganizationCommand(dto.OrganizationName, objectId);
            Result result = await _mediator.Send(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{organizationName}")]
        public async Task<IActionResult> GetOrganizationByName(string organizationName)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var command = new GetOrganizationByNameQuery(organizationName, objectId);
            GetOrganizationByNameDto organization = await _mediator.Send(command);
            return Ok(organization);
        }
    }
}
