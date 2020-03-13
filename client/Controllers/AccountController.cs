using System;
using System.Net.Http;
using System.Threading.Tasks;
using client.Command;
using client.Entities;
using client.Models;
using client.Models.CommandModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private AccountCommandHandler _cmd;

        public AccountController(ClientServerContext ctx,
                                 IHttpClientFactory clientFactory,
                                 IOptions<SyncSetting> syncSetting)
        {
            _cmd = new AccountCommandHandler(ctx, clientFactory, syncSetting.Value.CloudUrl);
        }

        [HttpPost]
        public async Task<bool> Add(AccountRequestModel req) => await _cmd.Handle(new AddAccountCommand(req));

        [HttpPut]
        public async Task<bool> Update(AccountRequestModel req) =>await _cmd.Handle(new UpdateAccountCommand(req));

        [HttpDelete]
        public async Task<bool> Delete(Guid id) =>await _cmd.Handle(new DeleteAccountCommand(id));
    }
}
