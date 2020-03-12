using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace client.Entities
{
    public class LogHistory
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public string Message { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public int RowNumber { get; set; }
        public DateTime LogDate { get; set; }
    }
}
