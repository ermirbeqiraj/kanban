﻿using System.ComponentModel.DataAnnotations;

namespace Issue.Data.Models
{
    public class InternalProjectModel
    {
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

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
