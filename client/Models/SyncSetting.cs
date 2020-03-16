namespace client.Models
{
    public class SyncSetting: ISyncSetting
    {
        public string CloudUrl { get; set; }
        public int SyncDuration { get; set; }
    }

    public interface ISyncSetting
    {
        string CloudUrl { get; set; }
        int SyncDuration { get; set; }
    }
}
