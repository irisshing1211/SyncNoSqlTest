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

        public bool SyncUpdate(AccountSyncUpdateRequestModel req)
        {
            // get histories since req.last sync
            var histories = _ctx.AccountHistories.AsQueryable()
                                .Where(a => (req.LastSync.HasValue && a.Time > req.LastSync.Value) || !req.LastSync.HasValue)
                                .ToList();

            // if count ==0
            if (histories.Count == 0)
            {
                // then update db and add history directly
                UpdateAccount(req.History);
            }
            else
            {
                // else if count >0
                // insert the req.history into the histories list + order by time
                // filter the list with accountid
                // check if any delete
                // if no then convert the data to object and run the list again to update the related account
            }
            var history = new AccountHistory
            {
                Time = req.History.Time,
                Action = req.History.Action,
                Data = req.History.Data,
                AccountId = req.History.AccountId ?? Guid.Empty
            };
            _ctx.AccountHistories.InsertOne(history);
            return false;
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
                        Dictionary<string, string> data =
                            JsonConvert.DeserializeObject<Dictionary<string, string>>(req.Data);

                        var update = _ctx.Accounts.Find(a => a.Id == req.AccountId).FirstOrDefault();

                        foreach (var field in data)
                        {
                            update.GetType().GetProperty(field.Key).SetValue(update, field.Value);
                        }

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

        #endregion
    }
}
