using System;
using System.Collections.Generic;
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
							//for (int i = 0; i < allGames.Length; i++) 
							//{
							//    if (allGames[i].Game == ticket.Game)
							//    {
							//        await DisplayAlert("FOUND", "FOUND", "OK");
							//        ticket.Name = allGames[i].Name;
							//        ticket.Price = allGames[i].Price;
							//        break;
							//    }
							//}
							if (state.Equals(Constants.End))
							{
								ScanSummaryPage.tickets.Add(ticket);
								await DisplayAlert("Added", ticket.ToString(), "OK");
							}
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
							if (state.Equals(Constants.End))
							{
								ScanSummaryPage.tickets.Add(ticket);
								await DisplayAlert("Added", ticket.ToString(), "OK");
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
