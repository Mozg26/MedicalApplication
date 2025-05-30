using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("MedicalTests")]
    [AssignedType(typeof(MedicalTestEntity))]
    public class MedicalTestEntity : BaseEntity
    {
        [Column("referral_id")]
        public int ReferralId { get; set; }

        [ForeignKey(nameof(ReferralId))]
        public ReferralEntity? ReferralEntity { get; set; }

        [Column("medical_test_type")]
        public string MedicalTestType { get; set; } = string.Empty;

        [Column("file_path")]
        public string FilePath { get; set; } = string.Empty;
    }
}
