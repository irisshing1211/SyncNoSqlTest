using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using client.Entities;
using client.Events;
using client.Models;
using client.Models.CommandModels;
using client.Repository;
using MongoDB.Driver;

namespace client.Command
{
    public class AccountCommandHandler
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ClientServerContext _ctx;
        private readonly string _url;
        private string InsertUrl => Path.Combine(_url, "AccountSync");
        public AccountCommandHandler(ClientServerContext ctx, IHttpClientFactory clientFactory, string url)
        {
            _ctx = ctx;
            _clientFactory = clientFactory;
            _url = url;

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
                var repo = new AccountRepository(_ctx);
                repo.Add(acc);

                await new UpdateSyncEvent(_clientFactory,
                                          new AccountSyncModel
                                          {
                                              Action = HistoryAction.Insert,
                                              Data = addAccEvent.History.Data,
                                              Time = acc.CreatedAt
                                          },
                                          InsertUrl,
                                          null).Push();

                return true;
            }
            else { return false; }
        }

        public async Task<bool> Handle(UpdateAccountCommand cmd)
        {
            var repo=new AccountRepository(_ctx);
            var old = repo.GetOne(cmd.AccountId);
            var update = new Account
            {
                Id=cmd.AccountId,
                Name = cmd.Name,
                Address = cmd.Address,
                Tel=cmd.Tel,
                CreatedAt = old.CreatedAt,
                UpdatedAt = DateTime.Now
            };

            if ( await new UpdateAccountEvent(_ctx, old, update).Push())
            {
                repo.Update(update);

                return true;
            }
            else { return false; }
        }

        public async Task<bool> Handle(DeleteAccountCommand cmd)
        {
            if (await new DeleteAccountEvent(_ctx, cmd.AccountId).Push())
            {
                var repo=new AccountRepository(_ctx);
                repo.Delete(cmd.AccountId);

                return true;
            }

            return false;
        }
    }
}
