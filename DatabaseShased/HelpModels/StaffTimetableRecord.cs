using DatabaseAbstractions.Models.DatabaseModels;

namespace DatabaseShared.HelpModels
{
    public class StaffTimetableRecord : BaseEntity
    {
        public int StaffId { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }
    }
}
