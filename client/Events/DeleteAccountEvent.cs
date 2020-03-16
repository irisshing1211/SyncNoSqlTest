using System;
using System.Threading.Tasks;
using client.Entities;
using client.Repository;

namespace client.Events
{
    /// <summary>
    /// add delete account history
    /// </summary>
    public class DeleteAccountEvent : BaseEvent, IEvents
    {
        private readonly Guid _id;

        public DeleteAccountEvent(ClientServerContext ctx, Guid id) : base(ctx) { _id = id; }

        public async Task<bool> Push()
        {
            var insert = new AccountHistory {Time = DateTime.Now, Action = HistoryAction.Delete, AccountId = _id};
            var repo = new AccountHistoryRepository(Ctx);
           await repo.AddHistory(insert);

            return true;
        }
    }
}
