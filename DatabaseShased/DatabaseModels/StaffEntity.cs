using DatabaseAbstractions.Models.Attributes;
using DatabaseShared.HelpModels.ForDatabase;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("Staff")]
    [AssignedType(typeof(StaffEntity))]
    public class StaffEntity : PersonInfoEntity
    {
        [Column("access_level")]
        public string AccessLevel { get; set; } = string.Empty;

        [Column("specialization")]
        public string Specialization {  get; set; } = string.Empty;

        [Column("work_experince")]
        public int WorkExperince {  get; set; }
    }
}
