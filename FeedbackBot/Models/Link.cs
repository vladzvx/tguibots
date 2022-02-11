using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FeedbackBot.Models
{
    public class Link
    {
        [BsonId]
        private ObjectId _id { get; } = ObjectId.GenerateNewId();
        public long ExternalMessageId { get; set; }
        public long ExternalChatId { get; set; }
        public long InternalMessageId { get; set; }
        public long InternalChatId { get; set; }
    }
}
