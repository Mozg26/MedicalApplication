using DatabaseAbstractions.Models.DatabaseModels;

namespace DatabaseShared.HelpModels.ForDatabase
{
    public class StaffTimetableRecordEntity : BaseEntity
    {
        public int StaffId { get; set; }

        public string TimeStart { get; set; }

        public string TimeEnd { get; set; }
    }
}
