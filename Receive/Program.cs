using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Program
    {
        private const string routingKey = "task_queue";
        public static void Main()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(routingKey, false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, args) =>
                    {
                        var message = Encoding.UTF8.GetString(args.Body);
                        Console.WriteLine($"Received {message}");
                    };
                    channel.BasicConsume("hello", true, consumer);
                    Console.WriteLine("Press ENTER to exit");
                    Console.ReadLine();
                }
                
            }
          
        }
    }
}