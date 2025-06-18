using MedicalApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace MedicalApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController(AppointmentManagmentService appointmentManagmentService) : Controller
    {
        private readonly AppointmentManagmentService _appointmentManagmentService = appointmentManagmentService;

        [HttpGet("slots")]
        public IActionResult GetAppointmentSlots([FromQuery] int doctorId, [FromQuery] DateTime date)
        {
            var stringDate = date.ToString("dd.MM.yyyy");

            var appointments = _appointmentManagmentService.GetDoctorAppointments(doctorId, stringDate);

            return Ok(appointments);
        }

        public IActionResult CreateAppointment([FromQuery] int doctorAppointmentId, [FromQuery] int patientId)
        {
            _appointmentManagmentService.CreateAppointment(patientId, doctorAppointmentId);

            return Ok();
        }
    }
}
