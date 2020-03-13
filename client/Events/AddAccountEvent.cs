using System.Threading.Tasks;
using client.Entities;
using Newtonsoft.Json;

namespace client.Events
{
    public class AddAccountEvent : BaseEvent, IEvents
    {
        private readonly Account _target;
        private AccountHistory _history;
        public AccountHistory History => _history;
        public AddAccountEvent(ClientServerContext ctx, Account account) : base(ctx) { _target = account; }

        public async Task<bool> Push()
        {
            var insert = new AccountHistory
            {
                Time = _target.CreatedAt, Data = JsonConvert.SerializeObject(_target), Action = HistoryAction.Insert
            };

          await  Ctx.AccountHistories.InsertOneAsync(insert);
          _history = insert;
            return true;
        }
    }
}
