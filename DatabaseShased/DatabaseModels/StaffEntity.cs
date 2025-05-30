using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Staff")]
    [AssignedType(typeof(StaffEntity))]
    public class StaffEntity : BaseEntity
    {
        [Column("person_info_id")]
        public int PersonInfoId { get; set; }

        [ForeignKey(nameof(PersonInfoId))]
        public PersonInfoEntity? PersonInfoEntity { get; set; }

        [Column("access_level")]
        public string AccessLevel { get; set; } = string.Empty;

        [Column("specialization")]
        public string Specialization {  get; set; } = string.Empty;

        [Column("work_experince")]
        public int WorkExperince {  get; set; }
    }
}
