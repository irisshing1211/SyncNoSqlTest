using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using client.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace client.Repository
{
    public class AccountHistoryRepository
    {
        private readonly ClientServerContext _ctx;

        public AccountHistoryRepository(ClientServerContext ctx) { _ctx = ctx; }

        public DateTime? LastSync()
        {
            var lastRecord = _ctx.AccountHistories.AsQueryable().OrderByDescending(a => a.Time).FirstOrDefault();

            return lastRecord?.Time;
        }

        public async Task<bool> AddHistories(List<AccountHistory> histories)
        {
            await _ctx.AccountHistories.InsertManyAsync(histories);

            return true;
        }

        public async Task<bool> AddHistory(AccountHistory history)
        {
            await _ctx.AccountHistories.InsertOneAsync(history);

            return true;
        }
    }
}
