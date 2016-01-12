namespace Jane.Data.EntityFramework.Entities
{
    public class UserSession
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }

        public virtual User User { get; set; }
        public virtual Session Session { get; set; }

    }
}
