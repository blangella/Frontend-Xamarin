﻿using System;
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
		GameNamePriceData[] allGames;

		public ScanSummaryPage()
		{
			InitializeComponent();
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			_restService = new RestService();

			switch (Constants.State)
			{
				case Constants.Start:
                    StartButton.IsEnabled = true;
                    if (Constants.startTickets?.Count > 0)
                    {
                        for (int i = 0; i<Constants.startTickets.Count; i++)
                        {
			                Constants.startTickets[i].Id = Constants.CreateTicketId(Constants.startTickets[i].Game, Constants.startTickets[i].Pack, Constants.startTickets[i].Nbr);
			                Constants.startTickets[i].Name = Constants.gamesAndNames[Constants.startTickets[i].Game];
                            Debug.Write("Checking bool ticket scan: "+Constants.startTickets[i].isScanned + "\n\tFrom: "+Constants.startTickets[i].Id);
                            if (Constants.startTickets[i].isScanned)
                            {
                                Debug.Write("Checking bool ticket scan TRUE: "+Constants.startTickets[i].isScanned + "\n\tFrom: "+Constants.startTickets[i].Id);
				                Constants.startTickets[i].Status = "Scanned";
                            }
                            else if (!Constants.startTickets[i].isScanned)
                            {
                                Debug.Write("Checking bool ticket scan FALSE: "+Constants.startTickets[i].isScanned + "\n\tFrom: "+Constants.startTickets[i].Id);
				                Constants.startTickets[i].Status = "Not Scanned";
                            }
                        }
						PacketListView.ItemsSource = null;
						PacketListView.ItemsSource = Constants.startTickets;
                    }
					break;
				case Constants.End:
				    StartButton.IsEnabled = false;
					PacketListView.ItemsSource = Constants.endTickets;
					break;
				case Constants.Inventory:
				    StartButton.IsEnabled = false;
					Constants.startTickets = await _restService.GetTicketsData(Constants.StockUpEndpoint, Constants.APIKey);
					PacketListView.ItemsSource = Constants.startTickets;
					break;
			}
		}

		async void Ticket_Tapped(object sender, EventArgs e)
		{
			if (((sender as Image).BindingContext is TicketData _selectedTicket))
			{
				await Navigation.PushModalAsync(new CustomScannerPage());
			}
		}

		async void Scan_Clicked(System.Object sender, System.EventArgs e)
		{
			await Navigation.PushModalAsync(new CustomScannerPage());
		}

		async void Start_Clicked(System.Object sender, System.EventArgs e)
		{
		    HttpResponseMessage response = await _restService.GetPreviousTicketsData();
		    string content = await response.Content.ReadAsStringAsync();
		    content = Constants.TakeOutHeaderJSON(content);
		    Constants.startTickets = JsonConvert.DeserializeObject<List<TicketData>>(content);
            for (int i = 0; i<Constants.startTickets.Count; i++)
            {
			    Constants.startTickets[i].Id = Constants.CreateTicketId(Constants.startTickets[i].Game, Constants.startTickets[i].Pack, Constants.startTickets[i].Nbr);
			    Constants.startTickets[i].Name = Constants.gamesAndNames[Constants.startTickets[i].Game];
				Constants.startTickets[i].isScanned = false;
				Constants.startTickets[i].Status = "Not Scanned";
            }
		    PacketListView.ItemsSource = Constants.startTickets;
		}

		async void Confirm_Clicked(System.Object sender, System.EventArgs e)
		{
			await DisplayAlert("Confirm All Scans", "Are you sure you want to confirm?", "OK");

			switch (Constants.State)
			{
				case Constants.End:
					for(int i = 0; i < Constants.endTickets?.Count; i++)
					{
						HttpResponseMessage response = await _restService.PostEndDayTicket(Constants.endTickets[i].Game.ToString(), Constants.endTickets[i].Pack.ToString(), Constants.endTickets[i].Nbr.ToString(), Constants.UserData.Emp_id);
						string content = await response.Content.ReadAsStringAsync();
						if (!response.IsSuccessStatusCode)
						{
							await DisplayAlert("Error", "Could not confirm ticket: " + Constants.endTickets[i].Id, "OK");
							break;
						}
					}
					break;
				case Constants.Start:
                    for (int i = 0; i < Constants.startTickets.Count; i++)
                    {
						if (Constants.startTickets[i].isScanned == false)
						{
							var action = await DisplayActionSheet("A ticket was not scanned.\n"+Constants.startTickets[i].Id, "Continue anyways", "Cancel");
							switch (action)
							{
								case "Continue anyways":
									break;
								case "Cancel":
									i = Constants.startTickets.Count;
									break;
							}
						}
                    }
                    for (int i = 0; i < Constants.startTickets.Count; i++)
                    {
						if (Constants.startTickets[i].isScanned)
						{
					        HttpResponseMessage response = await _restService.PostStartDayTicket(Constants.startTickets[i].Game.ToString(), Constants.startTickets[i].Pack.ToString(), Constants.startTickets[i].Nbr.ToString(), Constants.UserData.Emp_id);
					        string content = await response.Content.ReadAsStringAsync();
                            if (!response.IsSuccessStatusCode)
                            {
                                await DisplayAlert("Could not log ticket.", Constants.startTickets[i].Name + "\n" + Constants.startTickets[i].Name, "OK");
                            }
						}
                    }
                    await DisplayAlert("Logged all scanned tickets", "", "OK");
					break;
			}
		}
	}
}

