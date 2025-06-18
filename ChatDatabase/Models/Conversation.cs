namespace ChatDatabase.Models
{
    public class Conversation
    {
        public int ConversationId { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime LastMessageSendedDateTime { get; set; }
        public ICollection<Message> Messages { get; set; }
    }
}
