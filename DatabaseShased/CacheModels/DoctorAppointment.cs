using DatabaseShared.HelpModels.ForCache;

namespace DatabaseShared.CacheModels
{
    public class DoctorAppointment : StaffTimetableRecord
    {
        public string Date { get; set; }

        public int PatientId { get; set; }
    }
}
