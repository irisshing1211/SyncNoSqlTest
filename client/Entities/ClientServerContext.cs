using client.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace client.Entities
{
    public class ClientServerContext
    {
        private readonly IMongoDatabase _db = null;

        public ClientServerContext(IOptions<DbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);

            if (client != null)
                _db = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Account> Accounts => _db.GetCollection<Account>("Accounts");

        public IMongoCollection<AccountHistory> AccountHistories =>
            _db.GetCollection<AccountHistory>("AccountHistories");

        public IMongoCollection<LogHistory> Logs => _db.GetCollection<LogHistory>("Logs");
    }
}
