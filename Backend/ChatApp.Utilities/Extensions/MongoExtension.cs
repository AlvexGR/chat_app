using ChatApp.Entities.Common;
using MongoDB.Driver;

namespace ChatApp.Utilities.Extensions
{
    public static class MongoExtension
    {
        public static FilterDefinitionBuilder<T> GetBuilders<T>() where T : BaseModel
        {
            return Builders<T>.Filter;
        }

        public static string MongoIgnoreCase(this string source)
        {
            return $"/^{source}$/i";
        }
    }
}
