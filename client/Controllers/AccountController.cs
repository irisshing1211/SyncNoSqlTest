using System;
using client.Command;
using client.Entities;
using client.Models;
using client.Models.CommandModels;
using Microsoft.AspNetCore.Mvc;

namespace client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ClientServerContext _ctx;
        private AccountCommandHandler _cmd;

        public AccountController(ClientServerContext ctx)
        {
            _ctx = ctx;
            _cmd = new AccountCommandHandler(ctx);
        }

        [HttpPost]
        public bool Add(AccountRequestModel req) => _cmd.Handle(new AddAccountCommand(req));

        [HttpPut]
        public bool Update(AccountRequestModel req) => _cmd.Handle(new UpdateAccountCommand(req));

        [HttpDelete]
        public bool Delete(Guid id) => _cmd.Handle(new DeleteAccountCommand(id));
    }
}
