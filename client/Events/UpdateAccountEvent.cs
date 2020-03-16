using System.Collections.Generic;
using System.Threading.Tasks;
using client.Entities;
using client.Repository;
using Newtonsoft.Json;

namespace client.Events
{
    /// <summary>
    /// add update account history
    /// </summary>
    public class UpdateAccountEvent : BaseEvent, IEvents
    {
        private readonly Account _old, _target;
        private Dictionary<string, string> _updateList;
        public Dictionary<string, string> UpdateList => _updateList;

        public UpdateAccountEvent(ClientServerContext ctx, Account old, Account target) : base(ctx)
        {
            _old = old;
            _target = target;
        }

        public async Task<bool> Push()
        {
            Dictionary<string, string> updateList = new Dictionary<string, string>();
            var props = _old.GetType().GetProperties();

            foreach (var prop in props)
            {
                var propName = prop.Name;
                var oldVal = _old.GetType().GetProperty(propName).GetValue(_old);
                var newVal = _old.GetType().GetProperty(propName).GetValue(_target);

                if (!oldVal.Equals(newVal))
                    updateList.Add(propName, newVal.ToString());
            }

            _updateList = updateList;

            var insert = new AccountHistory
            {
                Time = _target.UpdatedAt,
                Action = HistoryAction.Update,
                AccountId = _target.Id,
                Data = JsonConvert.SerializeObject(updateList)
            };

            var repo = new AccountHistoryRepository(Ctx);
            await repo.AddHistory(insert);

            return false;
        }
    }
}
