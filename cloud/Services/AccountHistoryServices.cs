using System;
using System.Collections.Generic;
using System.Linq;
using cloud.Entities;
using cloud.Models;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace cloud.Services
{
    public class AccountHistoryServices : BaseServices
    {
        public AccountHistoryServices(CloudServerContext ctx) : base(ctx) {}

        public List<AccountHistory> GetHistories(DateTime from, DateTime? to)
        {
            var list = _ctx.AccountHistories.AsQueryable().Where(a => a.Time > from);

            if (to.HasValue)
                list = list.Where(a => a.Time <= to.Value);

            return list.OrderBy(a => a.Time).ToList();
        }

        public List<AccountHistory> SyncUpdate(AccountSyncUpdateRequestModel req)
        {
            // get histories since req.last sync
            var histories = _ctx.AccountHistories.AsQueryable()
                                .Where(a => (req.LastSync.HasValue && a.Time > req.LastSync.Value) ||
                                            !req.LastSync.HasValue)
                                .ToList();

            var history = new AccountHistory
            {
                Time = req.History.Time,
                Action = req.History.Action,
                Data = req.History.Data,
                AccountId = req.History.AccountId ?? Guid.Empty
            };

            // if count ==0 or is not updating the account
            if (histories.Count == 0 || req.History.Action != HistoryAction.Update)
            {
                // then update db and add history directly
                UpdateAccount(req.History);
            }

            // else
            else
            {
                // insert the req.history into the histories list + order by time
                var accountHistories = histories.Where(a => a.AccountId == history.AccountId).ToList();
                accountHistories.Add(history);

                // filter the list with accountid
                accountHistories = accountHistories.OrderBy(a => a.Time).ToList();

                // check if any delete, if no then
                if (accountHistories.All(a => a.Action != HistoryAction.Delete))
                {
                    // get account form db
                    var update = _ctx.Accounts.AsQueryable().FirstOrDefault(a => a.Id == req.History.AccountId);

                    // run the list again to update the related account

                    // foreach history in list
                    foreach (var h in histories) { update = UpdateAccountByData(update, h.Data); }

                    _ctx.Accounts.ReplaceOne(a => a.Id == update.Id, update);
                }
            }

            _ctx.AccountHistories.InsertOne(history);

            return histories;
        }

        #region function

        private bool UpdateAccount(AccountSyncModel req)
        {
            try
            {
                switch (req.Action)
                {
                    case HistoryAction.Insert:
                        var insert = JsonConvert.DeserializeObject<Account>(req.Data);
                        _ctx.Accounts.InsertOne(insert);

                        break;
                    case HistoryAction.Delete:
                        _ctx.Accounts.DeleteOne(a => a.Id == req.AccountId);

                        break;
                    case HistoryAction.Update:
                        var update = _ctx.Accounts.Find(a => a.Id == req.AccountId).FirstOrDefault();
                        update = UpdateAccountByData(update, req.Data);
                        _ctx.Accounts.ReplaceOne(a => a.Id == req.AccountId, update);

                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);

                return false;
            }
        }

        private Account UpdateAccountByData(Account account, string stored)
        {
            Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(stored);

            foreach (var field in data) { account.GetType().GetProperty(field.Key).SetValue(account, field.Value); }

            return account;
        }

        #endregion
    }
}
