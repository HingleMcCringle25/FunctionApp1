using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Net.Sockets;

namespace TicketHubFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Function1))]
        public async Task Run([QueueTrigger("tickets", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            string messageJson = message.MessageText;

            //deserialize json into an object
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var ticket = JsonSerializer.Deserialize<Ticket>(messageJson, options);

            if (ticket == null)
            {
                _logger.LogError("Failed to deserialize message");
                return;
            }

            _logger.LogInformation($"Ticket: " +
                $"{ticket.email} " +
                $"{ticket.name} " +
                $"{ticket.phone} " +
                $"{ticket.quantity} " +
                $"{ticket.creditCard} " +
                $"{ticket.creditExpire} " +
                $"{ticket.securityCode} " +
                $"{ticket.address} " +
                $"{ticket.city} " +
                $"{ticket.province} " +
                $"{ticket.postalCode} " +
                $"{ticket.country}");

            //add ticket info to database


            //get connection string from app settings
            string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("SQL Connection string is not set in the environment variables");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync();
                var query = "INSERT INTO dbo.TICKETHUB (concertId, email, name, phone, quantity, creditCard, creditExpire, securityCode, address, city, province, postalCode, country) VALUES (@concertId, @email, @name, @phone, @quantity, @creditCard, @creditExpire, @securityCode, @address, @city, @province, @postalCode, @country)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@email", ticket.email);
                    cmd.Parameters.AddWithValue("@name", ticket.name);
                    cmd.Parameters.AddWithValue("@phone", ticket.phone);
                    cmd.Parameters.AddWithValue("@quantity", ticket.quantity);
                    cmd.Parameters.AddWithValue("@creditCard", ticket.creditCard);
                    cmd.Parameters.AddWithValue("@creditExpire", ticket.creditExpire);
                    cmd.Parameters.AddWithValue("@securityCode", ticket.securityCode);
                    cmd.Parameters.AddWithValue("@address", ticket.address);
                    cmd.Parameters.AddWithValue("@city", ticket.city);
                    cmd.Parameters.AddWithValue("@province", ticket.province);
                    cmd.Parameters.AddWithValue("@postalCode", ticket.postalCode);
                    cmd.Parameters.AddWithValue("@country", ticket.country);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
