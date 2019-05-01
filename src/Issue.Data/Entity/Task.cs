using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Issue.Data.Entity
{
    public class Task
    {
        public Task()
        {
            Files = new HashSet<TaskFile>();
        }
        public int Id { get; set; }

        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(4000)]
        public string Description { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public int ProjectId { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<TaskFile> Files { get; set; }
        public virtual Project Project { get; set; }
    }
}
