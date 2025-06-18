using DatabaseShared.HelpModels.ForDatabase;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    public class DoctorAppointmentEntity : StaffTimetableRecordEntity
    {
        public string Date { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public PatientEntity? Patient { get; set; }
    }
}
