using DatabaseShared.CacheModels;

namespace MedicalApp.Services.Abstractions
{
    public interface IStaffScheduleService
    {
        void AddDoctorAppointmentSlot(DoctorAppointment slot);
        void RemoveDoctorAppointmentSlot(int slotId);
        void AddStaffAbsence(StaffAbsence absence);
        void RemoveStaffAbsence(int absenceId);
        void AddStaff(Staff staff);
        void RemoveStaff(int staffId);
    }

}
