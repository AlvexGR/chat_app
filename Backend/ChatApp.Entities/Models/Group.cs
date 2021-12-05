using System;
using System.Collections.Generic;
using ChatApp.Entities.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatApp.Entities.Models
{
    public class Group : BaseModel
    {
        public string Name { get; set; }

        public ICollection<Member> Members { get; set; }
    }

    public class Member
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public string NickName { get; set; }

        public DateTime JoinedAt { get; set; }
    }
}
