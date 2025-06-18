using DatabaseAbstractions.Models.DatabaseModels;
using DatabaseShared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.HelpModels.ForDatabase
{
    public class PersonInfoEntity : BaseEntity
    {
        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Column("pathronymic")]
        public string Pathronymic { get; set; } = string.Empty;

        [Column("birth_date")]
        public DateTime BirthDate { get; set; }

        [Column("photo")]
        public string Photo { get; set; } = string.Empty;

        [Column("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Column("sex")]
        public Sex Sex { get; set; }
    }
}
