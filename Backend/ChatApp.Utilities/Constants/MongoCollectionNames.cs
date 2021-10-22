using System.Collections.Generic;
using ChatApp.Entities.Models;

namespace ChatApp.Utilities.Constants
{
    public static class MongoCollectionNames
    {
        private static Dictionary<string, string> _collectionName = new Dictionary<string, string>
        {
            { nameof(User), "Users" }
        };

        public static string Get(string name)
        {
            return _collectionName[name];
        }
    }
}
