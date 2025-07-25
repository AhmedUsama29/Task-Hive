﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.Authentication;
using Shared.DataTransferObjects.Teams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TeamsController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpGet("get/{teamId}")]
        public async Task<ActionResult<TeamResponse>> GetTeamById([FromRoute] string teamId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(await _serviceManager.TeamService.GetTeamByIdAsync(teamId, userId!));
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TeamResponse>>> GetAllTeams()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _serviceManager.TeamService.GetAllTeamsByUserIdAsync(userId!));
        }

        [HttpPost("create")]
        public async Task<ActionResult<TeamResponse>> CreateTeam(TeamCreationDto teamCreationDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(await _serviceManager.TeamService.CreateTeamAsync(teamCreationDto, userId!));
        }

        [HttpPut("update/{teamId}")]
        public async Task<ActionResult<UserResponse>> UpdateTeamSettings([FromRoute] string teamId, TeamUpdateDto teamUpdateDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Ok(await _serviceManager.TeamService.UpdateTeamSettingsAsync(teamId, userId!, teamUpdateDto));
        }

        [HttpDelete("delete/{teamId}")]
        public Task DeleteTeam([FromRoute] string teamId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return _serviceManager.TeamService.DeleteTeamAsync(teamId, userId!);
        }

        [HttpPost("join")]
        public async Task<ActionResult<bool>> JoinTeam(string joinCode)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _serviceManager.TeamService.JoinTeamAsync(joinCode, userId!));
        }

        [HttpPost("leave/{teamId}")]
        public async Task<ActionResult<bool>> LeaveTeam([FromRoute] string teamId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(await _serviceManager.TeamService.LeaveTeamAsync(teamId, userId!));
        }
    }
}
