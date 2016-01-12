namespace Jane.Core.Models
{
    public class UserSessionModel 
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int SessionId { get; set; }

        public UserModel User { get; set; }
        public SessionModel Session { get; set; }
    }
}
