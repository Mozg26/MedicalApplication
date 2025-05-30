using DatabaseAbstractions.Models.DatabaseModels;
using DatabaseShared.DatabaseModels;
using DatabaseShared.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.HelpModels
{
    public class PersonInfo : BaseEntity
    {
        [Column("login")]
        public string Login { get; set; } = string.Empty;

        [ForeignKey(nameof(Login))]
        public LoginSystemEntity? LoginSystemEntity { get; set; }

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
