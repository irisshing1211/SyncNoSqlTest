using System.Collections.Generic;
using cloud.Entities;
using Newtonsoft.Json;

namespace cloud.Events
{
    public class AddAccountEvent : BaseEvent, IEvents
    {
        private readonly Account _target;

        public AddAccountEvent(CloudServerContext ctx, Account account) : base(ctx) { _target = account; }

        public bool Push()
        {
            var insert = new AccountHistory
            {
                Time = _target.CreatedAt, Data = JsonConvert.SerializeObject(_target), Action = HistoryAction.Insert
            };

            Ctx.AccountHistories.InsertOne(insert);

            return true;
        }
    }
}
