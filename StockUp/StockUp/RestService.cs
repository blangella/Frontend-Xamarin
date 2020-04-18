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
			var tblURL = URL + "tblUsers/" + id + "?" + "access_token="+Constants.APIKey;
			HttpResponseMessage response = await _client.GetAsync(tblURL);
			return response;
		}

		// GET a user data, RETURNS json response
		public async Task<UserData[]> GetUsersData(string URL, string id)
		{
			UserData[] usersData = null;
			id.Replace("@", "%40");
			var tblURL = URL + "tblUsers/?access_token="+Constants.APIKey;
			try
			{
				HttpResponseMessage response = await _client.GetAsync(tblURL);
				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					usersData = JsonConvert.DeserializeObject<UserData[]>(content);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("\tERROR {0}", ex.Message);
			}
			return usersData;
		}

		// GET all tickets from database, RETURNS list of ticket objects
		public async Task<List<TicketData>> GetTicketsData(string uri, string id)
		{
			List<TicketData> ticketsData = null;
			id.Replace("@", "%40");
			var getUrl = uri + "tblTickets?" + "access_token=" + id;
			try
			{
				HttpResponseMessage response = await _client.GetAsync(getUrl);
				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					ticketsData = JsonConvert.DeserializeObject<List<TicketData>>(content);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("\tERROR {0}", ex.Message);
			}

			return ticketsData;
		}

		// GET all tickets from database, RETURNS list of ticket objects
		public async Task<HttpResponseMessage> GetPreviousTicketsData()
		{

			var getUrl = Constants.StockUpEndpoint + "tblTickets/endDayPrevDayTickets?access_token=" + Constants.APIKey;
			HttpResponseMessage response = await _client.GetAsync(getUrl);
			return response;
			//try
			//            TicketData[] ticketsData = null;
			//{

			//    if (response.IsSuccessStatusCode)
			//    {
			//        string content = await response.Content.ReadAsStringAsync();
			//        ticketsData = JsonConvert.DeserializeObject<TicketData[]>(content);
			//    }
			//}
			//catch (Exception ex)
			//{
			//    Debug.WriteLine("\tERROR {0}", ex.Message);
			//}

			//return ticketsData;
		}



		// POST a user login, RETURNS json response with potential token
		public async Task<HttpResponseMessage> PostUserLogin(string URL, string email, string password)
		{
			URL += "Users/login";
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


		// DELETE a user data, RETURNS json response
		public async Task<HttpResponseMessage[]> DeleteUserData(string URL, string id, string email)
		{
			// POST into user auth model
			HttpClient authHttpClient;
			var authURL = URL + "Users/"+id+"?access_token="+Constants.APIKey;
			authHttpClient = new HttpClient();
			var authResponse = await authHttpClient.DeleteAsync(authURL);

			// POST into tblUser db model
			HttpClient tblHttpClient;
			var tblURL = URL + "tblUsers/"+email.Replace("@", "%40")+"?access_token"+Constants.APIKey;
			tblHttpClient = new HttpClient();
			var tblResponse = await tblHttpClient.DeleteAsync(tblURL);

			return new HttpResponseMessage[] { authResponse, tblResponse };
		}

		// FROM STORED PROCEDURES

		// POST a ticket to activated, RETURNS json response with header object
		public async Task<HttpResponseMessage> PostActivateTicket(string Game, string Pack, string Nbr, string Emp_id)
		{
			var URL = Constants.StockUpEndpoint+"tblTickets/activateTicket?access_token="+Constants.APIKey;
			var formContent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("Game", Game),
					new KeyValuePair<string, string>("Pack", Pack),
					new KeyValuePair<string, string>("Nbr", Nbr),
					new KeyValuePair<string, string>("Emp_id", Emp_id),
				});
			var myHttpClient = new HttpClient();
			var response = await myHttpClient.PostAsync(URL, formContent);

			return response;
		}

		// GET all games, RETURNS json response
		public async Task<HttpResponseMessage> GetAllGames()
		{
			HttpClient httpClient;
			var URL = Constants.StockUpEndpoint + "tblTickets/allGames?access_token=" + Constants.APIKey;
			httpClient = new HttpClient();
			var response = await httpClient.GetAsync(URL);

			return response;
		}

        // GET all games, RETURNS json response
		public async Task<HttpResponseMessage> GetInventory()
		{
			HttpClient httpClient;
			var URL = Constants.StockUpEndpoint + "tblTickets/inventoryTickets?access_token=" + Constants.APIKey;
			httpClient = new HttpClient();
			var response = await httpClient.GetAsync(URL);

			return response;
		}

		// GET daily counts, RETURNS json response
		public async Task<HttpResponseMessage> GetDailyCounts()
		{
			HttpClient httpClient;
			var URL = Constants.StockUpEndpoint + "tblTickets/dailyCounts?access_token=" + Constants.APIKey;
			httpClient = new HttpClient();
			var response = await httpClient.GetAsync(URL);

			return response;
		}

		// GET weekly counts, RETURNS json response
		public async Task<HttpResponseMessage> GetWeeklyCounts()
		{
			HttpClient httpClient;
			var URL = Constants.StockUpEndpoint + "tblTickets/weeklyCounts?access_token=" + Constants.APIKey;
			httpClient = new HttpClient();
			var response = await httpClient.GetAsync(URL);

			return response;
		}

		// GET monthly counts, RETURNS json response
		public async Task<HttpResponseMessage> GetMonthlyCounts()
		{
			HttpClient httpClient;
			var URL = Constants.StockUpEndpoint + "tblTickets/monthlyCounts?access_token=" + Constants.APIKey;
			httpClient = new HttpClient();
			var response = await httpClient.GetAsync(URL);

			return response;
		}

		// POST a ticket to seed into inventory, RETURNS json response with header object
		public async Task<HttpResponseMessage> PostSeedTicket(string Game, string Pack, string Nbr, string Emp_id)
		{
			var URL = Constants.StockUpEndpoint+"tblTickets/seedNewGame?access_token="+Constants.APIKey;
			var formContent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("Game", Game),
					new KeyValuePair<string, string>("Pack", Pack),
					new KeyValuePair<string, string>("Nbr", Nbr),
					new KeyValuePair<string, string>("Emp_id", Emp_id),
				});
			var myHttpClient = new HttpClient();
			var response = await myHttpClient.PostAsync(URL, formContent);

			return response;
		}

		// GET end day previous day tickets, RETURNS json response
		public async Task<HttpResponseMessage> GetEndDayPreviousDayTickets()
		{
			HttpClient httpClient;
			var URL = Constants.StockUpEndpoint + "tblTickets/endDayPrevDayTickets?access_token=" + Constants.APIKey;
			httpClient = new HttpClient();
			var response = await httpClient.GetAsync(URL);

			return response;
		}

		// POST a ticket to set as start day, RETURNS json response with header object
		public async Task<HttpResponseMessage> PostStartDayTicket(string Game, string Pack, string Nbr, string Emp_id)
		{
			var URL = Constants.StockUpEndpoint+"tblTickets/startDayTicket?access_token="+Constants.APIKey;
			var formContent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("Game", Game),
					new KeyValuePair<string, string>("Pack", Pack),
					new KeyValuePair<string, string>("Nbr", Nbr),
					new KeyValuePair<string, string>("Emp_id", Emp_id),
				});
			var myHttpClient = new HttpClient();
			var response = await myHttpClient.PostAsync(URL, formContent);

			return response;
		}

		// POST a ticket to set as end day, RETURNS json response with header object
		public async Task<HttpResponseMessage> PostEndDayTicket(string Game, string Pack, string Nbr, string Emp_id)
		{
			var URL = Constants.StockUpEndpoint+"tblTickets/endDayTicket?access_token="+Constants.APIKey;
			var formContent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("Game", Game),
					new KeyValuePair<string, string>("Pack", Pack),
					new KeyValuePair<string, string>("Nbr", Nbr),
					new KeyValuePair<string, string>("Emp_id", Emp_id),
				});
			var myHttpClient = new HttpClient();
			var response = await myHttpClient.PostAsync(URL, formContent);

			return response;
		}

		// GET group tickets, RETURNS json response
		public async Task<HttpResponseMessage> GetGroupTickets(string Game, string Pack, string Nbr)
		{
			HttpClient httpClient;
			var ticketURL = "Game="+Game+"&Pack="+Pack+"&Nbr="+Nbr+"&";
			var URL = Constants.StockUpEndpoint + "tblTickets/groupTicket?"+ticketURL+"access_token=" + Constants.APIKey;
			httpClient = new HttpClient();
			var response = await httpClient.GetAsync(URL);

			return response;
		}

		// DELETE an entire pack, RETURNS json response
		public async Task<HttpResponseMessage> PostToDeletePack(string Game, string Pack)
		{
			var URL = Constants.StockUpEndpoint+"tblTickets/pack?access_token="+Constants.APIKey;
			var formContent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("Game", Game),
					new KeyValuePair<string, string>("Pack", Pack),
				});
			var myHttpClient = new HttpClient();
			var response = await myHttpClient.PostAsync(URL, formContent);

			return response;
		}

		// GET group tickets, RETURNS json response
		public async Task<HttpResponseMessage> GetSparePacks(string Game)
		{
			HttpClient httpClient;
			var ticketURL = "Game="+Game+"&";
			var URL = Constants.StockUpEndpoint + "tblTickets/sparePacks?"+ticketURL+"access_token=" + Constants.APIKey;
			httpClient = new HttpClient();
			var response = await httpClient.GetAsync(URL);

			return response;
		}
	}
}
