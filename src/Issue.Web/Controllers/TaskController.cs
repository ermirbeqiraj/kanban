using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Issue.Data;
using Issue.Data.Models;
using Issue.Data.Repositories;
using Issue.Web.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Issue.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskRepository _taskRepo;
        private readonly ILogger _logger;

        private const int PAGE_SIZE = 20;

        public TaskController(ITaskRepository taskRepo, ILogger logger)
        {
            _taskRepo = taskRepo;
            _logger = logger;

        }

        [HttpGet]
        public async Task<IActionResult> List(int projectId, int? pageNumber)
        {
            if (!pageNumber.HasValue)
                pageNumber = 1;

            var items = await _taskRepo.GetAllTasksAsync();
            var items1 = items
            .OrderByDescending(x => x.Created)
            .Skip((pageNumber.Value - 1) * PAGE_SIZE)
            .Take(PAGE_SIZE);

            return Ok(items1);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var item = await _taskRepo.GetTaskByIdAsync(id);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskModels model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            model.CreatedBy = User.Identity.Name;
            await _taskRepo.CreateTaskAsync(model);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModels model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model is not valid");

            model.UpdatedBy = User.Identity.Name;
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