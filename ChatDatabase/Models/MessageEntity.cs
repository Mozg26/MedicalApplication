using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChatDatabase.Models
{
    public class MessageEntity
    {
        [Column("id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ConversationId { get; set; }


        [ForeignKey(nameof(ConversationId))]
        public ConversationEntity? Conversation { get; set; }

        public int UserId { get; set; }

        public string MessageText { get; set; }

        public DateTime DateTimeSended { get; set; }

        public bool IsRead { get; set; }
    }
}
