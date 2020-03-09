using System.Collections.Generic;
using Newtonsoft.Json;
using cloud.Entities;

namespace cloud.Events
{
    public class UpdateAccountEvent : BaseEvent, IEvents
    {
        private readonly Account _old, _target;

        public UpdateAccountEvent(CloudServerContext ctx, Account old, Account target) : base(ctx)
        {
            _old = old;
            _target = target;
        }

        public bool Push()
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

            var insert = new AccountHistory
            {
                Time = _target.UpdatedAt,
                Action = HistoryAction.Update,
                AccountId = _target.Id,
                Data = JsonConvert.SerializeObject(updateList)
            };

            Ctx.AccountHistories.InsertOne(insert);

            return false;
        }
    }
}
