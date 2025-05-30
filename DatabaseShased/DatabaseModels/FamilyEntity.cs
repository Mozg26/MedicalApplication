using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Families")]
    [AssignedType(typeof(FamilyEntity))]
    public class FamilyEntity : BaseEntity
    {
        [Column("patient_id")]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public PatientEntity? PatientEntity { get; set; }

        [Column("relation")]
        public string Relation { get; set; } = string.Empty;

        [Column("related_patient_id")]
        public int RelatedPatientId { get; set; }

        [ForeignKey(nameof(RelatedPatientId))]
        public PatientEntity? RelatedPatientEntity { get; set; }
    }
}
