using DatabaseAbstractions.Models.Attributes;
using DatabaseAbstractions.Models.DatabaseModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseShared.DatabaseModels
{
    [Table("LoginSystem")]
    [AssignedType(typeof(LoginSystemEntity))]
    public class LoginSystemEntity : BaseEntity
    {
        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
