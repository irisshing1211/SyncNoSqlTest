using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using client.Entities;
using client.Events;
using client.Models;
using client.Models.CommandModels;
using client.Repository;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace client.Command
{
    public class AccountCommandHandler
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ClientServerContext _ctx;
        private readonly string _url;
        private readonly AccountRepository _accRepo;
        private readonly DateTime? _lastSync;
        private string InsertUrl => Path.Combine(_url, "AccountSync");

        public AccountCommandHandler(ClientServerContext ctx, IHttpClientFactory clientFactory, string url)
        {
            _ctx = ctx;
            _clientFactory = clientFactory;
            _url = url;
            _accRepo = new AccountRepository(ctx);
            _lastSync = new AccountHistoryRepository(ctx).LastSync();
        }

        public async Task<bool> Handle(AddAccountCommand cmd)
        {
            Account acc = new Account
            {
                Id = Guid.NewGuid(),
                Name = cmd.Name,
                Tel = cmd.Tel,
                Address = cmd.Address,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var addAccEvent = new AddAccountEvent(_ctx, acc);

            if (await addAccEvent.Push())
            {
                _accRepo.Add(acc);

                return await SyncHistories(new AccountSyncModel
                {
                    Action = HistoryAction.Insert,
                    Data = addAccEvent.History.Data,
                    Time = acc.CreatedAt
                });
            }

            return false;
        }

        public async Task<bool> Handle(UpdateAccountCommand cmd)
        {
            var old = _accRepo.GetOne(cmd.AccountId);

            var update = new Account
            {
                Id = cmd.AccountId,
                Name = cmd.Name,
                Address = cmd.Address,
                Tel = cmd.Tel,
                CreatedAt = old.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            var updateEvent = new UpdateAccountEvent(_ctx, old, update);

            if (await updateEvent.Push())
            {
                _accRepo.Update(update);

                return await SyncHistories(new AccountSyncModel
                {
                    Action = HistoryAction.Update,
                    Data = JsonConvert.SerializeObject(updateEvent.UpdateList),
                    Time = update.UpdatedAt,
                    AccountId = update.Id
                });
            }

            return false;
        }

        public async Task<bool> Handle(DeleteAccountCommand cmd)
        {
            if (await new DeleteAccountEvent(_ctx, cmd.AccountId).Push())
            {
                _accRepo.Delete(cmd.AccountId);

                return true;
            }

            return false;
        }

        private async Task<bool> SyncHistories(AccountSyncModel model)
        {
            var syncEvent = new UpdateSyncEvent(_clientFactory, model, InsertUrl, _lastSync);

            if (await syncEvent.Push())
            {
                var updateEvent = new UpdateAccountFromSyncEvent(_ctx, syncEvent.Histories);

                if (await updateEvent.Push())
                    return true;
            }

            return false;
        }

        private async Task<bool> AddHistories(List<AccountHistory> histories)
        {
            var repo = new AccountHistoryRepository(_ctx);
            await repo.AddHistories(histories);

            return true;
        }
    }
}
