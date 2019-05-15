using Issue.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Issue.Data.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskModels>> GetAllTasksAsync();

        Task CreateTaskAsync(TaskModels model);

        Task UpdateTaskAsync(TaskModels model);

        Task DeleteTaskAsync(int id, string author);

        Task<TaskModels> GetTaskByIdAsync(int id);
    }


    public class TaskRepository : ITaskRepository
    {
        AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateTaskAsync(TaskModels model)
        {
            _context.Tasks.Add(new Entity.Task
            {
                Active = true,
                Created = DateTime.UtcNow,
                CreatedBy = model.CreatedBy,
                Description = model.Description,
                Title = model.Title

            });

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskModels model)
        {
            var dbTask = await _context.Tasks.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if (dbTask == null)
                throw new Exception("Project not found");

            dbTask.Description = model.Description;
            dbTask.Modified = DateTime.UtcNow;
            dbTask.Title = model.Title;
            dbTask.UpdatedBy = model.UpdatedBy;

            _context.Entry(dbTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id, string author)
        {
            var dbTask = await _context.Tasks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dbTask == null)
                throw new Exception("Project not found");

            dbTask.Active = false;
            dbTask.Modified = DateTime.UtcNow;
            dbTask.UpdatedBy = author;

            _context.Entry(dbTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<TaskModels>> GetAllTasksAsync()
        {
            return await _context.Tasks.Select(x => new TaskModels
            {
                Id = x.Id,
                CreatedBy = x.CreatedBy,
                Description = x.Description,
                Title = x.Title,
                UpdatedBy = x.UpdatedBy
            }).ToListAsync();
        }


        public async Task<TaskModels> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.Where(x => x.Id == id)
                .Select(x => new TaskModels
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    Description = x.Description,
                    Title = x.Title,
                    UpdatedBy = x.UpdatedBy
                }).FirstOrDefaultAsync();
        }


    }
}
