using DatabaseAbstractions.Models.CacheModels;

namespace DatabaseShared.CacheModels
{
    public class Appointment : CacheEntity
    {
        public DoctorAppointment? DoctorAppointment { get; set; }

        public string AppointmentType { get; set; } = string.Empty;

        public string AppointmentStatus { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;

        public string Prescription { get; set; } = string.Empty;

        public double Cost { get; set; }

        public string Cabinet { get; set; } = string.Empty;

        public string AppointmentGroup { get; set; } = string.Empty;
    }
}
