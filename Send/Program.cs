using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    class Program
    {
        private const string routingKey = "task_queue";
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };
            int attempt = 0;
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "hello", durable: false, exclusive: false, autoDelete: false, arguments: null);
                    do
                    {
                        string message = $"Hello World {attempt++}";
                        var body = Encoding.UTF8.GetBytes(message);
                        var properties = channel.CreateBasicProperties();
                        properties.Persistent = true;
                        channel.BasicPublish(exchange: "", routingKey: routingKey, basicProperties: null, body: body);
                        Console.WriteLine($"message {message} sent");
                    } while (Console.ReadLine() != "q");
                }
            }

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}