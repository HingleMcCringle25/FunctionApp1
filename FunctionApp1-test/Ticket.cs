using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketHubFunction
{
    public class Ticket
    {
        public int concertId { get; set; } = 0;


        public string email { get; set; } = string.Empty;


        public string name { get; set; } = string.Empty;


        public string phone { get; set; } = string.Empty;


        public int quantity { get; set; } = 0;


        public string creditCard { get; set; } = string.Empty;


        public string creditExpire { get; set; } = string.Empty;


        public string securityCode { get; set; } = string.Empty;


        public string address { get; set; } = string.Empty;


        public string city { get; set; } = string.Empty;

        public string province { get; set; } = string.Empty;

        public string postalCode { get; set; } = string.Empty;
        public string country { get; set; } = string.Empty;

    }
}
