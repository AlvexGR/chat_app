using ChatApp.Entities.Common;
using ChatApp.Entities.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Entities.Models
{
    public class ChatMessage : BaseModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string SenderId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string ReceiverId { get; set; }

        public ReceiverType ReceiverType { get; set; }

        public MessageType MessageType { get; set; }

        public string Message { get; set; }
    }
}
