using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatDatabase.Models
{
    public class ConversationEntity
    {
        [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime LastMessageSendedDateTime { get; set; }
    }
}
