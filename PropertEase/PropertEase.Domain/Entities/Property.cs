using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PropertEase.Domain.Entities
{
    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("address")]
        public string Address { get; set; }

        [BsonElement("price")]
        public Decimal128 Price { get; set; }

        [BsonElement("codeInternal")]
        public string CodeInternal { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("idOwner")]
        public ObjectId IdOwner { get; set; }

        [BsonElement("imageIds")]
        public List<ObjectId> ImageIds { get; set; } = new List<ObjectId>();

        [BsonElement("traceIds")]
        public List<ObjectId> TraceIds { get; set; } = new List<ObjectId>();

       
        [BsonIgnore]
        public Owner Owner { get; set; }

        [BsonIgnore]
        public List<PropertyImage> Images { get; set; } = new List<PropertyImage>();

        [BsonIgnore]
        public List<PropertyTrace> Traces { get; set; } = new List<PropertyTrace>();
    }
}
