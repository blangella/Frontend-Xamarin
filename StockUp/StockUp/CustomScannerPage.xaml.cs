using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using StockUp.Model;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using ZXing.Net.Mobile.Forms;

namespace StockUp
{
	public partial class CustomScannerPage : ContentPage
	{

		ZXingScannerView Zxing;
		ZXingDefaultOverlay Overaly;
		List<String> ScannedTickets = new List<String>();
		RestService _restService;
		GameNamePriceData[] allGames;
		public static string state; 

		public CustomScannerPage () : base ()
		{
			InitializeComponent();
			On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);

			Zxing = new ZXingScannerView
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				AutomationId = "zxingScannerView",
			};
			Zxing.OnScanResult += (result) =>
				Device.BeginInvokeOnMainThread(async () => {

				// Stop analysis until we navigate away so we don't keep reading barcodes
				Zxing.IsAnalyzing = false;

				// Show an alert
				//await DisplayAlert ("Scanned Barcode", result.Text, "OK");

				// Navigate away
				//await Navigation.PopAsync ();

				// Show pop up
				String[] actionButtons = { "Confirm", "Done" };
				var action = await DisplayActionSheet("Scanned Barcode\n" + result.Text, "Redo", "Cancel", actionButtons);
				TicketData ticket;
				switch (action)
					{
						case "Redo":
							Zxing.IsAnalyzing = true;
							break;
						case "Confirm":
							ticket = new TicketData
							{
								Game = Constants.GetGameNum(result.Text),
								Pack = Constants.GetPackNum(result.Text),
								Nbr = Constants.GetNbrNum(result.Text),
								Emp_id = Constants.UserData.Emp_id
							};
							ticket.Id = Constants.CreateTicketId(ticket.Game, ticket.Pack, ticket.Nbr);
                            switch (state)
                            {
								case Constants.Start:
									for (int i = 0; i<Constants.startTickets.Length; i++)
                                    {
                                        if (Constants.startTickets[i].Id.Equals(ticket.Id))
                                        {
											Constants.startTickets[i].isScanned = true;
                                        }
									}
									break;
								case Constants.Activate:
									break;
								case Constants.End:
								    ticket.Id = ticket.Game.ToString() + ticket.Pack.ToString() + ticket.Nbr.ToString();
								    ScanSummaryPage.tickets.Add(ticket);
									break;
								case Constants.Inventory:
									break;
                            }
                            await DisplayAlert("Added ticket", "", "OK");
							Zxing.IsAnalyzing = true;
							break;
						case "Done":
							ticket = new TicketData
							{
								Game = Constants.GetGameNum(result.Text),
								Pack = Constants.GetPackNum(result.Text),
								Nbr = Constants.GetNbrNum(result.Text),
								Emp_id = Constants.UserData.Emp_id
							};
							ticket.Id = Constants.CreateTicketId(ticket.Game, ticket.Pack, ticket.Nbr);

                            switch (state)
                            {
								case Constants.Start:
									for (int i = 0; i<Constants.startTickets.Length; i++)
                                    {
									    Debug.Write("\nComparing Scan: " + Constants.startTickets[i].Id + "\n\tWith ticket: " + ticket.Id);
                                        if (Constants.startTickets[i].Id.Equals(ticket.Id))
                                        {
											Constants.startTickets[i].isScanned = true;
											Debug.Write("Checking bool: "+Constants.startTickets[i].isScanned);
											break;
                                        }
									}
									break;
								case Constants.Activate:
									break;
								case Constants.End:
								    ScanSummaryPage.tickets.Add(ticket);
								    await DisplayAlert("Added", ticket.ToString(), "OK");
									break;
								case Constants.Inventory:
									break;
                            }
							await Navigation.PopModalAsync();
							break;

					}
				});

			Overaly = new ZXingDefaultOverlay
			{
				TopText = "Hold your phone up to the barcode",
				BottomText = "Scanning will happen automatically",
				ShowFlashButton = Zxing.HasTorch,
				AutomationId = "zxingDefaultOverlay",
			};
			Overaly.FlashButtonClicked += (sender, e) => {
				Zxing.IsTorchOn = !Zxing.IsTorchOn;
			};

			// The root page of your application
			ScannerGrid.Children.Add(Zxing);
			ScannerGrid.Children.Add(Overaly);
			//Content = grid;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Zxing.IsScanning = true;
		}

		protected override void OnDisappearing()
		{
			Zxing.IsScanning = false;

			base.OnDisappearing();
		}
	}
}
