using System;

namespace client.Models
{
    public class AccountSyncModel
    {
        public DateTime Time { get; set; }
        public Guid? AccountId { get; set; }
        public string Data { get; set; }
        public HistoryAction Action { get; set; }
    }
}
