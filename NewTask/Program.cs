using System;
using System.Text;
using Global;
using RabbitMQ.Client;


namespace NewTask
{
    class Program
    {
        static void Main(string[] args)
        {
            var message = GetMessage(args);
            var body = Encoding.UTF8.GetBytes(message);
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            int attempt = 0;
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(Global.Constants.RoutingKey, durable: false, exclusive: false, autoDelete: false, arguments: null);
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        channel.BasicPublish(exchange: "", routingKey: Constants.RoutingKey, basicProperties: properties, body: body);
                        Console.WriteLine($"message {message} sent");
                   
                }
            }
        }

        private static string GetMessage(string[] args)
        {
            return args.Length > 0 ? string.Join(" ", args) : "Hello World";

        }
    }
}