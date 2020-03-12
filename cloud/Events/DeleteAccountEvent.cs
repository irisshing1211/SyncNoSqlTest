using System;
using cloud.Entities;

namespace cloud.Events
{
    public class DeleteAccountEvent: BaseEvent, IEvents
    {
        private readonly Guid _id;

        public DeleteAccountEvent(CloudServerContext ctx, Guid id) : base(ctx) { _id = id; }

        public bool Push()
        {
            var insert = new AccountHistory {Time = DateTime.Now, Action = HistoryAction.Delete, AccountId = _id};
            Ctx.AccountHistories.InsertOne(insert);

            return true;
        }
    }
}
