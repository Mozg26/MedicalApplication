using DatabaseShared.CacheModels;

namespace DatabaseShared.CacheExtensions
{
    public static class DoctorAppointmentExtensions
    {
        public static IEnumerable<DoctorAppointment?> ByDate(this IEnumerable<DoctorAppointment?> doctorAppointments, int doctorId, string date)
        {
            return doctorAppointments
                    .Where(doctorAppointment => doctorAppointment?.Date == date && doctorAppointment.StaffId == doctorId)
                    .ToList();
        }
    }
}
