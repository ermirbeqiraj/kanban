using Issue.Data;
using Issue.Data.Models;
using Issue.Data.Repositories;
using Issue.Web.IdentityModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Issue.Web.Controllers
{
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectRepository _projectRepo;
        private readonly ILogger _logger;
        public ProjectsController(IProjectRepository projectRepo, ILogger<ProjectsController> logger)
        {
            _projectRepo = projectRepo;
            _logger = logger;
        }


        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> List()
        {
            var items = await _projectRepo.GetAllProjectsAsync();
            return Ok(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _projectRepo.GetProjectByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create([FromBody]InternalProjectModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            model.CreatedBy = User.GetUserName();
            await _projectRepo.CreateProjectAsync(model);
            return Ok();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Update([FromBody] InternalProjectModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            model.UpdatedBy = User.GetUserName();
            await _projectRepo.UpdateProjectAsync(model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _projectRepo.DeleteProjectAsync(id, User.Identity.Name);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete project :{id} with error: {ex.Message}");
                return BadRequest("Failed to delete project");
            }
        }
    }
}