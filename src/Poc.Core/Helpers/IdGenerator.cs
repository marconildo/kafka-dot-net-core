
using MongoDB.Bson;

namespace Poc.Core.Helpers
{
    public class IdGenerator
    {
        public static string GetNewId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        public static ObjectId GetObjectId()
        {
            return ObjectId.GenerateNewId();
        }
    }
}
