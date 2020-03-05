using System;

namespace cloud.Models
{
    public class AccountSyncUpdateRequestModel
    {
        public AccountSyncModel History { get; set; }
        public TimeSpan? LastSync { get; set; }
    }
}
