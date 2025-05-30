using DatabaseShared.HelpModels;

namespace DatabaseShared.DatabaseModels
{
    public class DoctorAppointmentEntity : StaffTimetableRecord
    {
        public string Date { get; set; }

        public int PatientId { get; set; }
    }
}
