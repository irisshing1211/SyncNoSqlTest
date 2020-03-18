using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using client.Entities;
using client.Repository;

namespace client.Query
{
    public class AccountQueryService : IQueryService<Account>
    {
        private readonly ClientServerContext _ctx;
        public AccountQueryService(ClientServerContext ctx) { _ctx = ctx; }

        public async Task<List<Account>> GetAll()
        {
            var repo = new AccountRepository(_ctx);

            return await repo.GetAll();
        }

        public Account GetById(Guid id)
        {
            var repo = new AccountRepository(_ctx);

            return repo.GetOne(id);
        }
    }
}
