using ChatDatabase.Models;
using ChatService.Services.Abstractions;
using ChatDatabase;

namespace ChatService.Services
{
    public class ChatService : IChatService
    {
        private readonly MainContext _context;
        public ChatService(MainContext context)
        {
            _context = context;
        }

        public async Task<Conversation> CreateConversationAsync(int userId1, int userId2)
        {
            var conversation = new ConversationEntity
            {
                UserId1 = userId1,
                UserId2 = userId2,
                DateTimeCreated = DateTime.UtcNow,
                LastMessageSendedDateTime = DateTime.UtcNow
            };
            _context.Conversations.Add(conversation);
            await _context.SaveChangesAsync();

            var conversation1 = new Conversation
            {
                UserId1 = userId1,
                UserId2 = userId2,
                DateTimeCreated = DateTime.UtcNow,
                LastMessageSendedDateTime = DateTime.UtcNow
            };

            return conversation1;
        }

        public async Task<Message> SendMessageAsync(int conversationId, int userId, string text)
        {
            var message = new MessageEntity
            {
                ConversationId = conversationId,
                UserId = userId,
                MessageText = text,
                DateTimeSended = DateTime.UtcNow,
                IsRead = false
            };
            _context.Messages.Add(message);

            var conversation = await _context.Conversations.FindAsync(conversationId);
            if (conversation != null)
            {
                conversation.LastMessageSendedDateTime = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            var message1 = new Message
            {
                ConversationId = conversationId,
                UserId = userId,
                MessageText = text,
                DateTimeSended = DateTime.UtcNow,
                IsRead = false
            };

            return message1;
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(int conversationId)
        {
            return new List<Message>();
        }
    }
}
