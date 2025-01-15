using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertEase.Domain.Entities
{
    public class PropertyImage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("file")]
        public string File { get; set; }

        [BsonElement("enabled")]
        public bool Enabled { get; set; }
    }
}
