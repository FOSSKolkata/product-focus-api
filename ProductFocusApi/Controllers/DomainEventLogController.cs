using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class DomainEventLogController : ControllerBase
    {
        private readonly Messages messages;
        [HttpGet("{id}")]
        public async Task<IActionResult> getEventLog(long id)
        {
            return Ok(id);
        }
    }
}
