using System;
using System.Collections.Generic;
using StockUp.Model;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace StockUp
{
	public partial class HomePage : ContentPage
	{
		RestService _restService;
		//ZXingScannerPage scanPage;
		public HomePage()
		{
			InitializeComponent();
            //NavigationPage.SetHasBackButton(this, false);
		}

		async void Start_Clicked(System.Object sender, System.EventArgs e)
		{
			ScanSummaryPage page = new ScanSummaryPage();
			Constants.State = Constants.Start;
			await Navigation.PushAsync(page);
		}

		async void Activate_Clicked(System.Object sender, System.EventArgs e)
		{
			Constants.State = Constants.Activate;
			await Navigation.PushModalAsync(new CustomScannerPage());
		}

		async void End_Clicked(System.Object sender, System.EventArgs e)
		{
			Constants.State = Constants.End;
			ScanSummaryPage page = new ScanSummaryPage();
			await Navigation.PushAsync(page);
		}

		async void Logout_Clicked(System.Object sender, System.EventArgs e)
		{
			_restService = new RestService();

			var response = await _restService.PostUserLogout(Constants.StockUpEndpoint, Constants.UserData.Email, Constants.UserData.Passsword, Constants.APIKey);
			var responseCode = response.IsSuccessStatusCode;

			if (responseCode)
			{
				NavigationPage page = new NavigationPage(new LoginPage());
				App.Current.MainPage = page;
				await Navigation.PopToRootAsync();
			}
			else
			{
				await DisplayAlert("Error", "Could not sign out", "OK");
			}
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

			_restService = new RestService();
            var response = await _restService.GetAllGames();
			string content = await response.Content.ReadAsStringAsync();
			content = Constants.TakeOutHeaderJSON(content);
			Constants.InitializeAllGames(content);

    //        switch (Constants.State)
    //        {
				//case Constants.Start:
				//	StartButton.IsEnabled = true;
				//	EndButton.IsEnabled = false;
				//	break;
				//case Constants.Activate:
				//	break;
				//case Constants.End:
				//	StartButton.IsEnabled = false;
				//	EndButton.IsEnabled = true;
				//	break;
				//default:
				//	StartButton.IsEnabled = true;
				//	EndButton.IsEnabled = false;
				//	break;
    //        }
        }
	}
}
