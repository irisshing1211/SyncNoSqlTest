using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using client.Command;
using client.Entities;
using client.Models;
using client.Models.CommandModels;
using client.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private AccountCommandHandler _cmd;
        private readonly IQueryService<Account> _queries;

        public AccountController(ClientServerContext ctx,
                                 IHttpClientFactory clientFactory,
                                 IQueryService<Account> queries,
                                 IOptions<SyncSetting> syncSetting)
        {
            _cmd = new AccountCommandHandler(ctx, clientFactory, syncSetting.Value.CloudUrl);
            _queries = queries;
        }

        [HttpGet("List")]
        public async Task<List<Account>> List() => await _queries.GetAll();

        [HttpGet("GetOne")]
        public Account GetOne(Guid id) => _queries.GetById(id);

        [HttpPost]
        public async Task<bool> Add(AccountRequestModel req) => await _cmd.Handle(new AddAccountCommand(req));

        [HttpPut]
        public async Task<bool> Update(AccountRequestModel req) => await _cmd.Handle(new UpdateAccountCommand(req));

        [HttpDelete]
        public async Task<bool> Delete(Guid id) => await _cmd.Handle(new DeleteAccountCommand(id));
    }
}
