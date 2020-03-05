using System;
using System.Runtime.CompilerServices;
using cloud.Entities;

namespace cloud.Services
{
    public class BaseServices
    {
        protected internal CloudServerContext _ctx;

        public BaseServices(CloudServerContext ctx) { _ctx = ctx; }
        
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
