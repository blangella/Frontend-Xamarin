using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Windows.Input;
using StockUp.Model;
using ZXing.Net.Mobile.Forms;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;

namespace StockUp
{

public partial class ScanSummaryPage : ContentPage
	{
		public static String state;
		//ZXingScannerPage scanPage;
		RestService _restService;
		public static List<TicketData> tickets = new List<TicketData>();
		GameNamePriceData[] allGames;
		Dictionary<int, String> gamesAndNames = new Dictionary<int, string>();
		Dictionary<int, String> gamesAndPrices = new Dictionary<int, string>();

		public ScanSummaryPage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			_restService = new RestService();
			HttpResponseMessage responseGames = await _restService.GetAllGames();
			string contentGames = await responseGames.Content.ReadAsStringAsync();
			contentGames = Constants.TakeOutHeaderJSON(contentGames);
			allGames = JsonConvert.DeserializeObject<GameNamePriceData[]>(contentGames);
			for (int j = 0; j < allGames.Length; j++)
			{
				GameNamePriceData curr = allGames[j];
				if (!gamesAndNames.ContainsKey(curr.Game))
				{
					gamesAndNames.Add(curr.Game, curr.Name);
					gamesAndPrices.Add(curr.Game, curr.Price);
				}
			}
			switch (state)
			{
				case Constants.Start:
					HttpResponseMessage response = await _restService.GetPreviousTicketsData();
					string content = await response.Content.ReadAsStringAsync();
					content = Constants.TakeOutHeaderJSON(content);
					TicketData[]ticketsData = JsonConvert.DeserializeObject<TicketData[]>(content);
					PacketListView.ItemsSource = ticketsData;
					break;
				case Constants.End:
					PacketListView.ItemsSource = tickets;
					break;
				case Constants.Inventory:
					ticketsData = await _restService.GetTicketsData(Constants.StockUpEndpoint, Constants.APIKey);
					PacketListView.ItemsSource = ticketsData;
					break;
			}
		}

		async void Ticket_Tapped(object sender, EventArgs e)
		{
			if (((sender as Image).BindingContext is TicketData _selectedTicket))
			{
				switch (state)
					{
						case Constants.Start:
							break;
						case Constants.End:
							CustomScannerPage.state = Constants.End;
							break;
						case Constants.Inventory:
							break;
					}
				await Navigation.PushModalAsync(new CustomScannerPage());
			}
		}

		async void Scan_Clicked(System.Object sender, System.EventArgs e)
		{
			switch (state)
				{
					case Constants.Start:
						break;
					case Constants.End:
						CustomScannerPage.state = Constants.End;
						break;
					case Constants.Inventory:
						break;
				}
			await Navigation.PushModalAsync(new CustomScannerPage());
		}

		async void Confirm_Clicked(System.Object sender, System.EventArgs e)
		{
			switch (state)
			{
				case Constants.End:
					for(int i = 0; i < tickets.Count; i++)
					{
						HttpResponseMessage response = await _restService.PostEndDayTicket(tickets[i].Game.ToString(), tickets[i].Pack.ToString(), tickets[i].Nbr.ToString(), Constants.UserData.Emp_id);
						string content = await response.Content.ReadAsStringAsync();
						if (!response.IsSuccessStatusCode)
						{
							await DisplayAlert("Error", "Could not confirm ticket: " + tickets[i].Id, "OK");
							break;
						}
					}
					break;
			}
			await DisplayAlert("Confirm All Scans", "Are you sure you want to confirm?", "OK");
		}
	}
}

