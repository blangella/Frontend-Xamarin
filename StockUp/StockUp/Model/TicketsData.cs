using System;
using Newtonsoft.Json;

namespace StockUp.Model
{
    public class TicketsData
    {
        [JsonProperty("")]
        public TicketData[] ticketsData { get; set; }
    }
}
