using System;
using Newtonsoft.Json;

namespace StockUp.Model
{
    public class LoginData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("ttl")]
        public string Ttl { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }
    }

    public class AuthData
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
