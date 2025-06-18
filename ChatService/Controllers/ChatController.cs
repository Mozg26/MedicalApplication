using ChatService.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;
        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateConversation(int userId1, int userId2)
        {
            var conversation = await _chatService.CreateConversationAsync(userId1, userId2);
            return Ok(conversation);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage(int conversationId, int userId, string messageText)
        {
            var message = await _chatService.SendMessageAsync(conversationId, userId, messageText);
            return Ok(message);
        }

        [HttpGet("{conversationId}/messages")]
        public async Task<IActionResult> GetMessages(int conversationId)
        {
            var messages = await _chatService.GetMessagesAsync(conversationId);
            return Ok(messages);
        }
    }
}
