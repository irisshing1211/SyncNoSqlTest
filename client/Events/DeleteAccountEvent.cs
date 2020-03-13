using System;
using System.Threading.Tasks;
using client.Entities;

namespace client.Events
{
    public class DeleteAccountEvent: BaseEvent, IEvents
    {
    private readonly Guid _id;

    public DeleteAccountEvent(ClientServerContext ctx, Guid id) : base(ctx) { _id = id; }

    public async  Task<bool> Push()
    {
        var insert = new AccountHistory {Time = DateTime.Now, Action = HistoryAction.Delete, AccountId = _id};
   await     Ctx.AccountHistories.InsertOneAsync(insert);

        return true;
    }
    }
}
