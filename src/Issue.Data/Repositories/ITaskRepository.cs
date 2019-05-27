using Issue.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Issue.Data.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskModel>> GetAllTasksAsync(int projectId);

        Task CreateTaskAsync(int projectId, TaskModel model);

        Task UpdateTaskAsync(TaskModel model);

        Task DeleteTaskAsync(int id, string author);

        Task<TaskModel> GetTaskByIdAsync(int id);
    }


    public class TaskRepository : ITaskRepository
    {
        AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateTaskAsync(int projectId, TaskModel model)
        {
            _context.Tasks.Add(new Entity.Task
            {
                Active = true,
                Created = DateTime.UtcNow,
                CreatedBy = model.CreatedBy,
                Description = model.Description,
                Title = model.Title,
                ProjectId = projectId,
                Modified = null
            });

            await _context.SaveChangesAsync();
        }

        public async Task UpdateTaskAsync(TaskModel model)
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

        public async Task<List<TaskModel>> GetAllTasksAsync(int projectId)
        {
            return await _context.Tasks.Where(a => a.ProjectId == projectId).Select(x => new TaskModel
            {
                Id = x.Id,
                CreatedBy = x.CreatedBy,
                Description = x.Description,
                Title = x.Title,
                UpdatedBy = x.UpdatedBy,
            }).ToListAsync();
        }


        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            return await _context.Tasks.Where(x => x.Id == id)
                .Select(x => new TaskModel
                {
                    Id = x.Id,
                    CreatedBy = x.CreatedBy,
                    Description = x.Description,
                    Title = x.Title,
                    UpdatedBy = x.UpdatedBy,
                    ProjectId = x.ProjectId

                }).FirstOrDefaultAsync();
        }


    }
}
