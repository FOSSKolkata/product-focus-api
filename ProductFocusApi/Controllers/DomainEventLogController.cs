using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocusApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class DomainEventLogController : ControllerBase
    {
        private readonly IMediator _mediator;
        public DomainEventLogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{productId}/{offset}/{count}/query")]
        public async Task<IActionResult> GetEventLog(long productId, long offset, long count, DateTime? startDate, DateTime? endDate, [FromQuery] IList<long> moduleIds, [FromQuery] IList<long> userIds, [FromQuery] string eventType)
        {
            List<GetDomainEventLogDto> eventLog = await _mediator.Send(new GetDomainEventLogQuery(productId, moduleIds, userIds, offset, count, startDate, endDate, eventType));
            return Ok(eventLog);
        }
    }
}

