using System;
using System.Threading.Tasks;
using cloud.Entities;
using cloud.Models;
using cloud.Services;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly CloudServerContext _ctx;
        private readonly AccountServices _service;

        public AccountController(CloudServerContext ctx)
        {
            _ctx = ctx;
            _service = new AccountServices(ctx);
        }

        [HttpPost]
        public bool Add(AccountRequestModel req) => _service.Add(req);

        [HttpPut]
        public Task<bool> Update(AccountRequestModel req) => _service.Update(req);

        [HttpDelete]
        public bool Delete(Guid id) => _service.Delete(id);
    }
}
