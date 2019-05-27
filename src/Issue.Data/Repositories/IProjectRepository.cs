using Issue.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Issue.Data.Repositories
{
    public interface IProjectRepository
    {
        Task<List<InternalProjectModel>> GetAllProjectsAsync();
        Task CreateProjectAsync(InternalProjectModel model);
        Task UpdateProjectAsync(InternalProjectModel model);
        Task DeleteProjectAsync(int id, string author);
        Task<InternalProjectModel> GetProjectByIdAsync(int id);
    }

    public class ProjectRepository : IProjectRepository
    {
        AppDbContext _context;
        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateProjectAsync(InternalProjectModel model)
        {
            
            _context.Projects.Add(new Entity.Project
            {
                Active = true,
                Created = DateTime.UtcNow,
                CreatedBy = model.CreatedBy,
                Description = model.Description,
                Name = model.Name,
                ProductionUrl = model.ProductionUrl,
                RepositoryUrl = model.RepositoryUrl,
                StagingUrl = model.StagingUrl
            });

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProjectAsync(int id, string author)
        {
            var dbProject = await _context.Projects.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (dbProject == null)
                throw new Exception("Project not found!");

            dbProject.Active = false;
            dbProject.Modified = DateTime.UtcNow;
            dbProject.UpdatedBy = author;

            _context.Entry(dbProject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<InternalProjectModel>> GetAllProjectsAsync()
        {
            return await _context.Projects.Select(x => new InternalProjectModel
            {
                Id = x.Id,
                CreatedBy = x.CreatedBy,
                Description = x.Description,
                Name = x.Name,
                ProductionUrl = x.ProductionUrl,
                RepositoryUrl = x.RepositoryUrl,
                StagingUrl = x.StagingUrl,
                UpdatedBy = x.UpdatedBy
            }).ToListAsync();
        }

        public async Task<InternalProjectModel> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.Where(x => x.Id == id)
                                .Select(x => new InternalProjectModel
                                {
                                    Id = x.Id,
                                    CreatedBy = x.CreatedBy,
                                    Description = x.Description,
                                    Name = x.Name,
                                    ProductionUrl = x.ProductionUrl,
                                    RepositoryUrl = x.RepositoryUrl,
                                    StagingUrl = x.StagingUrl,
                                    UpdatedBy = x.UpdatedBy
                                }).FirstOrDefaultAsync();
        }

        public async Task UpdateProjectAsync(InternalProjectModel model)
        {
            var dbProject = await _context.Projects.Where(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (dbProject == null)
                throw new Exception("Project not found!");

            dbProject.Description = model.Description;
            dbProject.Modified = DateTime.UtcNow;
            dbProject.Name = model.Name;
            dbProject.ProductionUrl = model.ProductionUrl;
            dbProject.RepositoryUrl = model.RepositoryUrl;
            dbProject.StagingUrl = model.StagingUrl;
            dbProject.UpdatedBy = model.UpdatedBy;

            _context.Entry(dbProject).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
