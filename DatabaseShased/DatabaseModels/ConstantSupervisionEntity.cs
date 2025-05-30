using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("ConstantSupervisions")]
    [AssignedType(typeof(ConstantSupervisionEntity))]
    public class ConstantSupervisionEntity : BaseEntity
    {
        [Column("patient_id")]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public PatientEntity? PatientEntity { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;
    }
}
