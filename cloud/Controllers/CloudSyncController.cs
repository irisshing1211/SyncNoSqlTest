using System;
using System.Collections.Generic;
using cloud.Entities;
using cloud.Models;
using cloud.Services;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountSyncController : ControllerBase
    {
        private readonly CloudServerContext _ctx;
        private readonly AccountHistoryServices _historyService;

        public AccountSyncController(CloudServerContext ctx)
        {
            _ctx = ctx;
            _historyService = new AccountHistoryServices(ctx);
        }

        [HttpGet]
        public List<AccountHistory> Sync(DateTime from, DateTime? to) => _historyService.GetHistories(from, to);

        [HttpPost]
        public List<AccountHistory> UpdateSync(AccountSyncUpdateRequestModel req)=>  _historyService.SyncUpdate(req);
    }
}
