using DatabaseAbstractions.Models.CacheModels;

namespace DatabaseShared.HelpModels.ForCache
{
    public class StaffTimetableRecord : CacheEntity
    {
        public int StaffId { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }
    }
}
