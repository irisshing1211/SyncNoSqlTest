using System;
using client.Entities;
using client.Events;
using client.Models.CommandModels;
using client.Repository;
using MongoDB.Driver;

namespace client.Command
{
    public class AccountCommandHandler
    {
        private readonly ClientServerContext _ctx;
        public AccountCommandHandler(ClientServerContext ctx) { _ctx = ctx; }

        public bool Handle(AddAccountCommand cmd)
        {
            Account acc=new Account
            {
                Id=Guid.NewGuid(),
                 Name = cmd.Name,
                 Tel = cmd.Tel, Address = cmd.Address,
                 CreatedAt = DateTime.Now,
                 UpdatedAt = DateTime.Now
            };

            if (new AddAccountEvent(_ctx, acc).Push())
            {
                var repo = new AccountRepository(_ctx);
                repo.Add(acc);

                return true;
            }
            else { return false; }
        }

        public bool Handle(UpdateAccountCommand cmd)
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

            if (new UpdateAccountEvent(_ctx, old, update).Push())
            {
                repo.Update(update);

                return true;
            }
            else { return false; }
        }

        public bool Handle(DeleteAccountCommand cmd)
        {
            if (new DeleteAccountEvent(_ctx, cmd.AccountId).Push())
            {
                var repo=new AccountRepository(_ctx);
                repo.Delete(cmd.AccountId);

                return true;
            }

            return false;
        }
    }
}
