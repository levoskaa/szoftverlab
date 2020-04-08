namespace api.DAL.EfDbContext
{
    public class DbTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Done { get; set; }
        public int StatusId { get; set; }
        public virtual DbStatus Status { get; set; }
    }
}
