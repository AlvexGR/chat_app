using System.Collections.Generic;
using ChatApp.Entities.Models;

namespace ChatApp.Utilities.Constants
{
    public static class MongoCollectionNames
    {
        private static readonly Dictionary<string, string> CollectionName = new()
        {
            { nameof(User), "Users" },
            { nameof(Group), "Groups" },
            { nameof(Friend), "Friends" },
            { nameof(FriendRequest), "FriendRequests" },
            { nameof(ChatMessage), "ChatMessages" },
        };

        public static string Get(string name)
        {
            return CollectionName[name];
        }
    }
}
