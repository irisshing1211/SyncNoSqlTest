using System;

namespace cloud.Models
{
    public class AccountSyncUpdateRequestModel
    {
        public AccountSyncModel History { get; set; }
        public DateTime? LastSync { get; set; }
    }
}
