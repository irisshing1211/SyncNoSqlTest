using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using client.Entities;
using client.Repository;
using Newtonsoft.Json;

namespace client.Events
{
    public class UpdateAccountFromSyncEvent : IEvents
    {
        private readonly List<AccountHistory> _histories;
        private readonly AccountRepository _repository;

        public UpdateAccountFromSyncEvent(ClientServerContext ctx, List<AccountHistory> histories)
        {
            _histories = histories;
            _repository = new AccountRepository(ctx);
        }

        public async Task<bool> Push()
        {
            // foreach history
            foreach (var history in _histories) { UpdateAccount(history); }

            return true;
        }

        private bool UpdateAccount(AccountHistory req)
        {
            try
            {
                switch (req.Action)
                {
                    case HistoryAction.Insert:
                        var insert = JsonConvert.DeserializeObject<Account>(req.Data);
                        _repository.Add(insert);

                        break;
                    case HistoryAction.Delete:
                        _repository.Delete(req.AccountId);

                        break;
                    case HistoryAction.Update:
                        var update = _repository.GetOne(req.AccountId);
                        update = UpdateAccountByData(update, req.Data);
                        _repository.Update(update);

                        break;
                }

                return true;
            }
            catch (Exception ex) { return false; }
        }

        private Account UpdateAccountByData(Account account, string stored)
        {
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(stored);

            foreach (var field in data) { account.GetType().GetProperty(field.Key).SetValue(account, field.Value); }

            return account;
        }
    }
}
