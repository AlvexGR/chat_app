using System.Collections.Generic;
using System.Linq;

namespace ChatApp.Utilities.Extensions
{
    public static class CollectionExtension
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> list) where T : class
        {
            return list == null || !list.Any();
        }
    }
}
