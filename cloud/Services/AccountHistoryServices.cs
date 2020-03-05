using System;
using System.Collections.Generic;
using System.Linq;
using cloud.Entities;
using cloud.Models;
using MongoDB.Driver;

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
            // if count ==0
            // then update db and add history directly
            
            // else if count >0
            // insert the req.history into the histories list + order by time
            // filter the list with accountid
            // check if any delete
            // if no then convert the data to object and run the list again to update the related account
            return false;
        }
    }
}
