﻿using Microsoft.AspNetCore.Mvc;
using ProductFocus.AppServices;
using ProductFocus.Dtos;
using CSharpFunctionalExtensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;
using ProductFocus.Domain.Common;

namespace ProductFocusApi.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly Messages _messages;

        public ProductController(Messages messages)
        {
            _messages = messages;
        }


        [HttpPost("{id}")]
        public async Task<IActionResult> AddModule(long id, [FromBody] AddModuleDto dto)
        {
            var command = new AddModuleCommand(id, dto.Name);
            Result result = await _messages.Dispatch(command);
            return result.IsSuccess ? Ok() : BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModulesByProductId(long id)
        {
            List<GetModuleDto> moduleList = await _messages.Dispatch(new GetModuleListQuery(id));
            return Ok(moduleList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetKanbanViewByProductId(long id)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            List<GetKanbanViewDto> kanbanViewList = await _messages.Dispatch(new GetKanbanViewQuery(id, objectId));
            return Ok(kanbanViewList);
        }

        [HttpGet("{id}/{groupCategoryEnum}/query")]
        public async Task<IActionResult> GetKanbanViewByProductIdAndQuery(long id, [FromQuery] long? SprintId, [FromQuery] List<long> UserIds, GroupCategoryEnum groupCategoryEnum)
        {
            string objectId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            GetKanbanViewListDto kanban = await _messages.Dispatch(new GetKanbanViewFilterQuery(id, objectId, SprintId, UserIds, groupCategoryEnum));
            return Ok(kanban);
        }
    }
}
