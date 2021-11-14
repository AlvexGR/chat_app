using ChatApp.Entities.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Entities.Models
{
    public class Friend : BaseModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId1 { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId2 { get; set; }
    }
}
