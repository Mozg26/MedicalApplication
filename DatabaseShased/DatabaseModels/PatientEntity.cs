using DatabaseAbstractions.Models.Attributes;
using DatabaseShared.Enums;
using DatabaseShared.HelpModels.ForDatabase;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Patients")]
    [AssignedType(typeof(PatientEntity))]
    public class PatientEntity : PersonInfoEntity
    {
        [Column("identification_document_number")]
        public string IdentificationDocumentNumber { get; set; } = string.Empty;

        [Column("insurance_card_number")]
        public string InsuranceCardNumber { get; set; } = string.Empty;

        [Column("medical_inshurance_number")]
        public string MedicalInshuranceNumber { get; set; } = string.Empty;

        [Column("blood_type")]
        public BloodType BloodType { get; set; }

        [Column("ph_factor")]
        public PhFactor PhFactor { get; set; }

        [Column("city")]
        public string City {  get; set; } = string.Empty;
    }
}
