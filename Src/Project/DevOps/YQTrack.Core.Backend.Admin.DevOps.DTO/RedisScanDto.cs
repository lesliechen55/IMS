using System.Collections.Generic;

namespace YQTrack.Core.Backend.Admin.DevOps.DTO
{
    public static class RedisScanDto
    {
        public static BatchDeleteState BatchDeleteState { get; set; }

        public static DeleteData DeleteData { get; set; } = new DeleteData();
    }

    public class DeleteData
    {
        public string Filter { get; set; }
        public string Msg { get; set; }

        public List<string> BatchDeleteKeys = new List<string>();
    }

    public enum BatchDeleteState
    {
        None = 0,
        Running = 1,
        Cancelling = 2,
        Cancelled = 3,
        Completed = 4
    }
}
