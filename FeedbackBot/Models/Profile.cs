using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackBot.Models
{
    public class Profile
    {
        [BsonId]
        public long BotId { get; set; }
        public long TargetChat { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
