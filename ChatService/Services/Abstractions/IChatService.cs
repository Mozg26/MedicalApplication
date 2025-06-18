using ChatDatabase.Models;

namespace ChatService.Services.Abstractions
{
    public interface IChatService
    {
        Task<Conversation> CreateConversationAsync(int userId1, int userId2);
        Task<Message> SendMessageAsync(int conversationId, int userId, string text);
        Task<IEnumerable<Message>> GetMessagesAsync(int conversationId);
    }
}
