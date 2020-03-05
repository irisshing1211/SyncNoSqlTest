using System;

namespace cloud.Models
{
    public class AccountSyncModel
    {
        public TimeSpan Time { get; set; }
        public Guid? AccountId { get; set; }
        public string Data { get; set; }
        public HistoryAction Action { get; set; }
    }
}
