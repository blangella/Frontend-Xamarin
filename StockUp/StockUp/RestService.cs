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

        public async Task<HttpResponseMessage[] > PostNewAdmin(string URL, string email, string first, string last, string password)
        {
            HttpClient authHttpClient;
            HttpClient tblHttpClient;
            
            // post into user auth model
            var authURL = URL + "/Users";
            var authFormContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                });
            authHttpClient = new HttpClient();
            var authResponse = await authHttpClient.PostAsync(authURL, authFormContent);

            // post into tblUser db model
            var tblURL = URL + "/tblUsers";
            var tblFormContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("IsAdmin", "1"),
                    new KeyValuePair<string, string>("Emp_id", email),
                    new KeyValuePair<string, string>("First", first),
                    new KeyValuePair<string, string>("Last", last),
                    new KeyValuePair<string, string>("Email", email),
                    new KeyValuePair<string, string>("Password", password),
                });
            tblHttpClient = new HttpClient();
            var tblResponse = await tblHttpClient.PostAsync(tblURL, tblFormContent);

            return new HttpResponseMessage[] { authResponse, tblResponse };
        }

        public async Task<HttpResponseMessage> GetUserData(string URL, string id)
        {
            id.Replace("@", "%40");
            var tblURL = URL + "/tblUsers/" + id + "?" + "access_token="+Constants.APIKey;
            HttpResponseMessage response = await _client.GetAsync(tblURL);
            return response;
        }
    }
}
