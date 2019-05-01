using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Issue.Data.Entity
{
    public class Project
    {
        public Project()
        {
            Tasks = new HashSet<Task>();
        }
        public int Id { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(300)]
        public string RepositoryUrl { get; set; }

        [MaxLength(300)]
        public string StagingUrl { get; set; }

        [MaxLength(300)]
        public string ProductionUrl { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool Active { get; set; }

        public virtual ICollection<Task> Tasks { get; set; }
    }
}
