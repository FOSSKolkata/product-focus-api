using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ProductFocusApi.Controllers._Subdomain.ProductTest
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductTestRunController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductTestRunController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("testPlanId")]
        public async Task<IActionResult> CreateTestRun(long testPlanId)
        {
            /*var command = new */
            return Ok();
        }
    }
}
