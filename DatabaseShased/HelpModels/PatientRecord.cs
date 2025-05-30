using DatabaseAbstractions.Models.DatabaseModels;
using DatabaseShared.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.HelpModels
{
    public class PatientRecord : BaseEntity
    {
        [Column("patient_id")]
        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public PatientEntity? PatientEntity { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;
    }
}
