using RabbitMQ.Client;
using System.Text;

namespace MessageBroker
{
    public interface IMessagePublisher
    {
        void Publish(string routingKey, string message);
    }

    public interface IModel { }

    public class RabbitMqPublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string ExchangeName = "app_exchange";
        
        public RabbitMqPublisher(string hostname)
        {
            var factory = new ConnectionFactory() { HostName = hostname };
            _connection = factory.CreateConnectionAsync();
            _channel = _connection.CreateModel();
        
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Topic, durable: true);
        
            // Очереди и привязки можно создавать здесь или отдельно
            _channel.QueueDeclare("auth_queue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare("main_queue", durable: true, exclusive: false, autoDelete: false);
            _channel.QueueDeclare("chat_queue", durable: true, exclusive: false, autoDelete: false);
        
            _channel.QueueBind("auth_queue", ExchangeName, "auth.*");
            _channel.QueueBind("main_queue", ExchangeName, "main.*");
            _channel.QueueBind("chat_queue", ExchangeName, "chat.*");
        }

        public void Publish(string routingKey, string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;
            
            _channel.BasicPublish(exchange: ExchangeName,
                                  routingKey: routingKey,
                                  basicProperties: properties,
                                  body: body);
        }

        public void Dispose()
        {
           _channel?.Close();
           _connection?.Close();
        }
    }
}
