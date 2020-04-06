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

        // GET a user data, RETURNS json response
        public async Task<HttpResponseMessage> GetUserData(string URL, string id)
        {
            id.Replace("@", "%40");
            Debug.Write("TEST USER "+URL);
            Debug.Write("TEST USER " + id);
            var tblURL = URL + "tblUsers/" + id + "?" + "access_token="+Constants.APIKey;
            Debug.Write("TEST URL: " + tblURL);
            HttpResponseMessage response = await _client.GetAsync(tblURL);
            return response;
        }

        // GET all tickets from database, RETURNS list of ticket objects
        public async Task<TicketData[]> GetTicketsData(string uri, string id)
        {
            TicketData[] ticketsData = null;
            id.Replace("@", "%40");
            var getUrl = uri + "tblTickets?" + "access_token=" + id;
            try
            {
                HttpResponseMessage response = await _client.GetAsync(getUrl);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ticketsData = JsonConvert.DeserializeObject<TicketData[]>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\tERROR {0}", ex.Message);
            }

            return ticketsData;
        }

        // POST a user login, RETURNS json response with potential token
        public async Task<HttpResponseMessage> PostUserLogin(string URL, string email, string password)
        {
            URL += "Users/login";
            Debug.Write("TEST"+URL);
            Debug.Write("TEST"+email + " " + password);
            var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                });
            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync(URL, formContent);

            return response;
        }

        // POST a user logout, RETURNS empty response code 204 if success
        public async Task<HttpResponseMessage> PostUserLogout(string URL, string email, string password, string id)
        {
            URL += "Users/logout?access_token=" + id;
            var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                });
            var myHttpClient = new HttpClient();
            var response = await myHttpClient.PostAsync(URL, formContent);

            return response;
        }

        // POST a new admin user, RETURNS array of json responses from two API post requests
        public async Task<HttpResponseMessage[] > PostNewUser(string URL, string email, string first, string last, string password, string IsAdmin)
        {
            // POST into user auth model
            HttpClient authHttpClient;
            var authURL = URL + "/Users";
            var authFormContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("email", email),
                    new KeyValuePair<string, string>("password", password),
                });
            authHttpClient = new HttpClient();
            var authResponse = await authHttpClient.PostAsync(authURL, authFormContent);
            var authContent = await authResponse.Content.ReadAsStringAsync();
            AuthData authData = JsonConvert.DeserializeObject<AuthData>(authContent);

            // POST into tblUser db model
            HttpClient tblHttpClient;
            var tblURL = URL + "/tblUsers";
            var tblFormContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("IsAdmin", IsAdmin),
                    new KeyValuePair<string, string>("Emp_id", authData.Id),
                    new KeyValuePair<string, string>("First", first),
                    new KeyValuePair<string, string>("Last", last),
                    new KeyValuePair<string, string>("Email", email),
                    new KeyValuePair<string, string>("Password", password),
                });
            tblHttpClient = new HttpClient();
            var tblResponse = await tblHttpClient.PostAsync(tblURL, tblFormContent);

            return new HttpResponseMessage[] { authResponse, tblResponse };
        }
    }
}
