using DatabaseAbstractions.DataRepository;
using MedicalApp.Services;
using Microsoft.AspNetCore.Mvc;
using DatabaseShared.CacheModels;
using DatabaseShared.CacheExtensions;

namespace MedicalApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController(JSONDataService jsonDataService, DataRepository dataRepository) : Controller
    {
        private readonly JSONDataService _jsonDataService = jsonDataService;

        private readonly DataRepository _dataRepository = dataRepository;

        [HttpGet("specializations")]
        public IActionResult GetSpecializations()
        {
            var specializations = _jsonDataService.SpecializationDirectory.Specializations;

            return Ok(specializations);
        }

        [HttpGet("doctors/bySpecialization")]
        public IActionResult GetDoctorsBySpecialization([FromQuery] string specialization)
        {
            if (string.IsNullOrWhiteSpace(specialization))
                return BadRequest("Специализация не может быть пустой.");

            var staff = _dataRepository.FindOrDefaultEntities<Staff>();

            if (staff == null)
                return Ok("Сотрудники клиники не найдены.");

            var doctors = staff.BySpecialization(specialization);

            if (doctors == null || !doctors.Any())
                return NotFound($"Врачи со специализацией '{specialization}' не найдены.");

            return Ok(doctors);
        }
    }
}
