using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Referrals")]
    [AssignedType(typeof(ReferralEntity))]
    public class ReferralEntity : BaseEntity
    {
        [Column("appointment_id")]
        public int AppointmentId { get; set; }

        [ForeignKey(nameof(AppointmentId))]
        public AppointmentEntity? AppointmentEntity { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("date_time")]
        public DateTime DateTime { get; set; }
    }
}
