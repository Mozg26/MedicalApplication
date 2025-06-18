using DatabaseShared.CacheModels;
using MedicalApp.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedicalApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class StaffScheduleController : ControllerBase
    {
        private readonly IStaffScheduleService _scheduleService;

        public StaffScheduleController(IStaffScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // Добавить слот записи врача
        [HttpPost("doctor-appointment")]
        public async Task<IActionResult> AddDoctorAppointmentSlot([FromBody] DoctorAppointment slot)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _scheduleService.AddDoctorAppointmentSlot(slot);
            return Ok("Слот записи добавлен");
        }

        // Удалить слот записи врача
        [HttpDelete("doctor-appointment/{slotId}")]
        public async Task<IActionResult> RemoveDoctorAppointmentSlot(int slotId)
        {
            _scheduleService.RemoveDoctorAppointmentSlot(slotId);
            return Ok("Слот записи удален");
        }

        // Добавить слот отсутствия сотрудника
        [HttpPost("staff-absence")]
        public async Task<IActionResult> AddStaffAbsence([FromBody] StaffAbsence absence)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _scheduleService.AddStaffAbsence(absence);
            return Ok("Слот отсутствия добавлен");
        }

        // Удалить слот отсутствия сотрудника
        [HttpDelete("staff-absence/{absenceId}")]
        public async Task<IActionResult> RemoveStaffAbsence(int absenceId)
        {
            _scheduleService.RemoveStaffAbsence(absenceId);
            return Ok("Слот отсутствия удален");
        }

        // Добавить сотрудника
        [HttpPost("staff")]
        public async Task<IActionResult> AddStaff([FromBody] Staff staff)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _scheduleService.AddStaff(staff);
            return Ok("Сотрудник добавлен");
        }

        // Удалить сотрудника
        [HttpDelete("staff/{staffId}")]
        public async Task<IActionResult> RemoveStaff(int staffId)
        {
            _scheduleService.RemoveStaff(staffId);
            return Ok("Сотрудник удален");
        }
    }
}
