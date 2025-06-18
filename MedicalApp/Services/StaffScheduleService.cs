using DatabaseAbstractions.DataRepository.Abstractions;
using DatabaseShared.CacheModels;
using MedicalApp.Services.Abstractions;

namespace MedicalApp.Services
{
    public class StaffScheduleService : IStaffScheduleService
    {
        private readonly IDataRepository _dataRepository;

        public StaffScheduleService(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public void AddDoctorAppointmentSlot(DoctorAppointment slot)
        {
            _dataRepository.SaveEntity(slot);
        }

        public void RemoveDoctorAppointmentSlot(int slotId)
        {
            var slot = _dataRepository.FindOrDefaultEntity<DoctorAppointment>(slotId);
            _dataRepository.RemoveEntity(slot);
        }

        public void AddStaffAbsence(StaffAbsence absence)
        {
            _dataRepository.SaveEntity(absence);
        }

        public void RemoveStaffAbsence(int absenceId)
        {
            var slot = _dataRepository.FindOrDefaultEntity<StaffAbsence>(absenceId);
            _dataRepository.RemoveEntity(slot);
        }

        public void AddStaff(Staff staff)
        {
            _dataRepository.SaveEntity(staff);
        }

        public void RemoveStaff(int staffId)
        {
            var staff = _dataRepository.FindOrDefaultEntity<Staff>(staffId);
            _dataRepository.RemoveEntity(staff);
        }
    }
}
