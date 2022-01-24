using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using ProductDocumentations.Application.Dtos;
using ProductDocumentations.CommandHandlers;
using ProductFocus.AppServices;
using System.Threading.Tasks;

namespace ProductDocumentation.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    //[Authorize]
    public class ProductDocumentationController : ControllerBase
    {
        private readonly Messages _messages;
        public ProductDocumentationController(Messages messages)
        {
            _messages = messages;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductDocumentation([FromBody] ProductDocumentationDto dto)
        {
            var command = new AddProductDocumentationCommand(dto.ParentId, dto.ProductId, dto.Title, dto.Description);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }
    }
}
