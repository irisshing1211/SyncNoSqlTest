using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cloud.Entities;
using cloud.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace cloud.Services
{
    public class AccountServices : BaseServices
    {
        public AccountServices(CloudServerContext ctx) : base(ctx) {}

        public bool SyncUpdate() { return false; }

        public List<Account> GetList() => _ctx.Accounts.AsQueryable().ToList();

        public bool Add(AccountRequestModel req)
        {
            try
            {
                var insert = new Account
                {
                    Name = req.Name,
                    Address = req.Address,
                    Tel = req.Tel,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _ctx.Accounts.InsertOne(insert);
                AddHistory(insert);

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);

                return false;
            }
        }

        public async Task<bool> Update(AccountRequestModel req)
        {
            try
            {
                var update = await _ctx.Accounts.AsQueryable().FirstOrDefaultAsync(a => a.Id == req.Id.Value);

                var old = new Account
                {
                    Id = update.Id,
                    Address = update.Address,
                    Name = update.Name,
                    Tel = update.Tel,
                    CreatedAt = update.CreatedAt,
                    UpdatedAt = update.UpdatedAt
                };

                update.Address = req.Address;
                update.Name = req.Name;
                update.Tel = req.Tel;
                update.UpdatedAt = DateTime.Now;
                _ctx.Accounts.ReplaceOne(a => a.Id == req.Id, update);
                UpdateHistory(old, update);

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);

                return false;
            }
        }

        public bool Delete(Guid id)
        {
            try
            {
                _ctx.Accounts.DeleteOne(a => a.Id == id);
                DeleteHistory(id);

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);

                return false;
            }
        }

        #region history

        private bool AddHistory(Account acc)
        {
            var insert = new AccountHistory
            {
                Time = acc.CreatedAt, Data = JsonConvert.SerializeObject(acc), Action = HistoryAction.Insert
            };

            _ctx.AccountHistories.InsertOne(insert);

            return true;
        }

        private bool UpdateHistory(Account old, Account update)
        {
            Dictionary<string, string> updateList = new Dictionary<string, string>();
            var props = old.GetType().GetProperties();

            foreach (var prop in props)
            {
                var propName = prop.Name;
                var oldVal = old.GetType().GetProperty(propName).GetValue(old);
                var newVal = update.GetType().GetProperty(propName).GetValue(update);

                if (!oldVal.Equals(newVal))
                    updateList.Add(propName, newVal.ToString());
            }

            var insert = new AccountHistory
            {
                Time = update.UpdatedAt,
                Action = HistoryAction.Update,
                AccountId = update.Id,
                Data = JsonConvert.SerializeObject(updateList)
            };

            _ctx.AccountHistories.InsertOne(insert);

            return false;
        }

        private bool DeleteHistory(Guid id)
        {
            var insert = new AccountHistory {Time = DateTime.Now, Action = HistoryAction.Delete, AccountId = id};
            _ctx.AccountHistories.InsertOne(insert);

            return true;
        }

        #endregion
    }
}
