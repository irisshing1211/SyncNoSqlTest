using System;

namespace client.Models
{
    public class SyncRequestModel : ISyncModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
