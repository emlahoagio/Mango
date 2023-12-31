﻿using System.Text;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Mango.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private string connectionString = "Endpoint=sb://quandtm-mangoweb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=5IrB7HoBuUZF51So9+e9Eo/wfDcBr5MFH+ASbCEO2ho=";



        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            try
            {
                await using var client = new ServiceBusClient(connectionString);

                ServiceBusSender sender = client.CreateSender(topic_queue_Name);

                var jsonMessage = JsonConvert.SerializeObject(message);

                ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                };

                await sender.SendMessageAsync(finalMessage);
                await client.DisposeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}

