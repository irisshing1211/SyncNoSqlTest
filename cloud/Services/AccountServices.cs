using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cloud.Entities;
using cloud.Events;
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
                new AddAccountEvent(_ctx,insert).Push();

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
                new UpdateAccountEvent(_ctx, old, update).Push();

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
                new DeleteAccountEvent(_ctx, id).Push();

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);

                return false;
            }
        }
        
    }
}
