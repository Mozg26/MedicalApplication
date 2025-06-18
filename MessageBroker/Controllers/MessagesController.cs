using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MessageBroker.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagePublisher _publisher;

        public MessagesController(IMessagePublisher publisher)
        {
            _publisher = publisher;
        }

        [HttpPost("send")]
        public IActionResult SendMessage([FromBody] MessageDto message)
        {
            _publisher.Publish(message.Type, message.Content);
            return Ok("Message sent");
        }
    }

    public class MessageDto
    {
        public string Type { get; set; }
        public string Content { get; set; }
    }
}
