using System;
using System.Globalization;
using System.Text;
using System.Threading;
using Global;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Worker
{
    class Program
    {
        public static void Main(string[] mainArgs)
        {
            Console.WriteLine($"Worker {mainArgs[0]}");
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(Constants.RoutingKey, false, false, false, null);
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, args) =>
                    {
                        var message = Encoding.UTF8.GetString(args.Body);
                        Console.WriteLine($"Received {message}");
                        int dots = message.Split('.').Length - 1;
                        Thread.Sleep(dots * 1000);
                        Console.WriteLine("Done");
                        channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
                    };
                    channel.BasicConsume(Constants.RoutingKey, false, consumer);
                    Console.WriteLine("Press ENTER to exit");
                    Console.ReadLine();
                }
                
            }
          
        }
    }
}