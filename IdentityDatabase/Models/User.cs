using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityDatabase.Models
{
    public class User
    {
        [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public byte[] Salt { get; set; }

        public bool IsLocked { get; set; }

        public int FailedLoginAttempts { get; set; }

        public DateTime LastLoginDate { get; set; }
    }
}
