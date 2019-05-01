using System.ComponentModel.DataAnnotations;

namespace Issue.Data.Entity
{
    public class File
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string DisplayName { get; set; }
        [MaxLength(10)]
        public string Extention { get; set; }
        [MaxLength(200)]
        public string MimeType { get; set; }
        public bool Active { get; set; }
    }
}
