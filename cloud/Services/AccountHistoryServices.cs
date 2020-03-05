using System;
using System.Collections.Generic;
using System.Linq;
using cloud.Entities;
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
    }
}
