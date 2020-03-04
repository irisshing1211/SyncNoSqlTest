using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace cloud.Entities
{
    public class Account
    {
        [BsonId(IdGenerator = typeof(GuidGenerator))]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
    }
}
