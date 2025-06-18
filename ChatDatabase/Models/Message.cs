namespace ChatDatabase.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int ConversationId { get; set; }
        public int UserId { get; set; }
        public string MessageText { get; set; }
        public DateTime DateTimeSended { get; set; }
        public bool IsRead { get; set; }
        public Conversation Conversation { get; set; }
    }
}
