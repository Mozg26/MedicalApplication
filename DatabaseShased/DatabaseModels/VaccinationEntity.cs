using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using DatabaseShared.HelpModels.ForDatabase;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Vaccinations")]
    [AssignedType(typeof(VaccinationEntity))]
    public class VaccinationEntity : PatientRecordEntity
    {
        [Column("vaccination_type")]
        public string VaccinationType { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("date")]
        public DateTime Date { get; set; } 
    }
}
