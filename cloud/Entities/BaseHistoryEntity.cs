using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace cloud.Entities
{
    public class BaseHistoryEntity
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }
        /// <summary>
        /// log time
        /// </summary>
        public TimeSpan Time { get; set; }
        /// <summary>
        /// data json string
        /// </summary>
        public string Data { get; set; }
        /// <summary>
        /// log action
        /// </summary>
        public HistoryAction Action { get; set; }
    }
}
