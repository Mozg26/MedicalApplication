using DatabaseAbstractions.DataRepository.Abstractions;
using DatabaseShared.CacheExtensions;
using DatabaseShared.CacheModels;
using System.Runtime.CompilerServices;
namespace MedicalApp.Services
{
    public class AppointmentManagmentService
    {
        private readonly IDataRepository _dataRepository;

        public void CreateAppointment(int patientId, int doctorAppointmentId)
        {
            var doctorAppointment = _dataRepository.FindOrDefaultEntity<DoctorAppointment>(doctorAppointmentId);

            doctorAppointment.PatientId = patientId;

            _dataRepository.SaveEntity(doctorAppointment);

            var appointment = new Appointment()
            {
                DoctorAppointment = doctorAppointment,
                AppointmentType = "nabludenie",
                AppointmentStatus = "created"
            };

            _dataRepository.SaveEntity(appointment);
        }

        public IEnumerable<DoctorAppointment> GetDoctorAppointments(int doctorId, string date)
        {
            var doctorAppointments = _dataRepository.FindOrDefaultEntities<DoctorAppointment>();

            var appointments = doctorAppointments.ByDate(doctorId, date);

            return appointments;
        }
    }
}
