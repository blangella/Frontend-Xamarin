using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using StockUp.Model;

namespace StockUp
{
    public class RestService
    {
        HttpClient _client;

        public RestService()
        {
            _client = new HttpClient();
        }

        public async Task<TicketData> GetTicketData(string uri)
        {
            TicketData ticketData = null;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ticketData = JsonConvert.DeserializeObject<TicketData>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return ticketData;
        }

        public async Task<HttpResponseMessage> PostUserLogin(string URL, string email, string password)
        {
            var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                });

            var myHttpClient = new HttpClient();
            //
            var response = await myHttpClient.PostAsync(URL, formContent);

            return response;
            //Events result = JsonConvert.DeserializeObject<Events>(json);
        }
    }
}
