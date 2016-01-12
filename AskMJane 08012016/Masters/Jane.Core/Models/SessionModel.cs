using System;

namespace Jane.Core.Models
{
    public class SessionModel :BaseModel
    {
        public string Token { get; set; }
        public DateTimeOffset? LastSeen { get; set; }
    }
}
