using System;
using System.Runtime.CompilerServices;
using client.Entities;
using MongoDB.Driver;

namespace client.Repository
{
    public class AccountRepository
    {
        private readonly ClientServerContext _ctx;

        public AccountRepository(ClientServerContext ctx) { _ctx = ctx; }
        public Account GetOne(Guid id) => _ctx.Accounts.Find(a => a.Id == id).FirstOrDefault();
        public bool Add(Account insert)
        {
            _ctx.Accounts.InsertOne(insert);

            return true;
        }

        public bool Update(Account update)
        {
            _ctx.Accounts.ReplaceOne(a=>a.Id==update.Id, update);
            return true;
        }

        public bool Delete(Guid id)
        {
            _ctx.Accounts.DeleteOne(a => a.Id == id);

            return true;
        }
        
        public virtual void WriteLog(string msg, [CallerMemberName] string method = null,
                                     [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            var controller = sourceFilePath.Substring(sourceFilePath.LastIndexOf("\\") + 1);

            LogHistory insert = new LogHistory
            {
                Class = controller,
                Method = method,
                RowNumber = sourceLineNumber,
                Message = msg,
                LogDate = DateTime.Now
            };
            _ctx.Logs.InsertOne(insert);
        }
    }
}
