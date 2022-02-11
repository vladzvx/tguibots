using MongoDB.Bson.Serialization.Attributes;

namespace TGUI.CoreLib.Models
{
    public class User
    {
        [BsonId]
        public long Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
    }
}
