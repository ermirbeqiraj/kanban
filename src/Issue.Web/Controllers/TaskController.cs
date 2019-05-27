using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Issue.Data;
using Issue.Data.Models;
using Issue.Data.Repositories;
using Issue.Web.IdentityModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Issue.Web.Controllers
{

    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ILogger _logger;
        private readonly UserManager<ApplicationUser> _userManager;


        public TaskController(UserManager<ApplicationUser> userManager, ITaskRepository taskRepo, ILogger<TaskController> logger)
        {
            _taskRepo = taskRepo;
            _logger = logger;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> List([FromQuery]int projectId)
        {
            var items = await _taskRepo.GetAllTasksAsync(projectId);

            return Ok(items);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var item = await _taskRepo.GetTaskByIdAsync(id);
            var item2 = item;
            return Ok(item);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateTask([FromBody] TaskModel model, [FromQuery] int projectId)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            model.CreatedBy = User.GetUserName();
            if (model.CreatedBy == null)
            {
                return BadRequest("Ju nuk jeni loguar me nje user");
            }
            await _taskRepo.CreateTaskAsync(projectId, model);

            return Ok();
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateTask([FromQuery] int projectId,[FromBody] TaskModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            model.UpdatedBy = User.GetUserName();
            await _taskRepo.UpdateTaskAsync(model);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskRepo.DeleteTaskAsync(id, User.Identity.Name);
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