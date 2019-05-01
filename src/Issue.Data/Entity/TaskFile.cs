namespace Issue.Data.Entity
{
    public class TaskFile
    {
        public int TaskId { get; set; }
        public int FileId { get; set; }

        public virtual Task Task { get; set; }
        public virtual File File { get; set; }
    }
}
