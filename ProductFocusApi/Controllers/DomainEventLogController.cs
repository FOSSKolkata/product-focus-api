using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocusApi.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductFocus.Domain.Common;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class DomainEventLogController : ControllerBase
    {
        private readonly Messages _messages;
        public DomainEventLogController(Messages messages)
        {
            _messages = messages;
        }

        [HttpGet("{productId}/{offset}/{count}/query")]
        public async Task<IActionResult> GetEventLog(long productId, long offset, long count, DateTime? startDate, DateTime? endDate, [FromQuery] IList<long> moduleIds, [FromQuery] IList<long> userIds, [FromQuery] string eventType)
        {
            List<GetDomainEventLogDto> eventLog = await _messages.Dispatch(new GetDomainEventLogQuery(productId, moduleIds, userIds, offset, count, startDate, endDate, eventType));
            return Ok(eventLog);
        }
    }
}

