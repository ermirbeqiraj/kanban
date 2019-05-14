using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Issue.Data;
using Issue.Data.Models;
using Issue.Web.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Issue.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        private const int PAGE_SIZE = 20;

        public TaskController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> List(int projectId, int? pageNumber)
        {
            if (!pageNumber.HasValue)
                pageNumber = 1;

            var list = await _context.Tasks.Select(x => new TaskModels
            {
                Id = x.Id,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                Description = x.Description,
                Modified = x.Modified.Value,
                ProjectId = x.ProjectId,
                Title = x.Title,
                UpdatedBy = x.UpdatedBy
            })
            .OrderByDescending(x => x.Created)
            .Skip((pageNumber.Value -1) * PAGE_SIZE)
            .Take(PAGE_SIZE)
            .ToListAsync();

            return Ok(list);
        }

        [HttpGet]
        public async Task<IActionResult> GetTaskById(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Model State is not valid");
            }

            var model = await _context.Tasks.Where(a => a.Id == id).Select(x => new TaskModels
            {
                Id = x.Id,
                Created = x.Created,
                CreatedBy = x.CreatedBy,
                Description = x.Description,
                Modified = x.Modified.Value,
                ProjectId = x.ProjectId,
                Title = x.Title,
                UpdatedBy = x.UpdatedBy

            }).FirstOrDefaultAsync();

            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskModels model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State is not valid");
            }


            model.Created = DateTime.UtcNow;
            model.CreatedBy = User.Identity.Name;

            await _context.AddAsync(model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody] TaskModels model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model State is not valid");
            }

            model.Modified = DateTime.UtcNow;
            model.UpdatedBy = "";

            _context.Update(model);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int? id)
        {
            if (!id.HasValue)
            {
                return BadRequest("Model State is not Valid");
            }

            var model = await _context.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
            model.Active = false;
            _context.Update(model);
            return Ok();
        }
    }
}