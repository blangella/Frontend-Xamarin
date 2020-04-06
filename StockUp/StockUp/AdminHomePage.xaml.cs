using System;
using System.Collections.Generic;

using Xamarin.Forms;
using StockUp.Model;

namespace StockUp
{
    public partial class AdminHomePage : ContentPage
    {
        RestService _restService;
        public AdminHomePage()
        {
            InitializeComponent();
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
        
        async void Inventory_Clicked(System.Object sender, System.EventArgs e)
        {
            ScanSummaryPage page = new ScanSummaryPage
            {
                state = Constants.Inventory
            };
            await Navigation.PushAsync(page);
        }

        async void ManageUsers_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ManageUsersPage());
        }

        async void Analytics_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AnalyticsPage());
        }
    }
}
