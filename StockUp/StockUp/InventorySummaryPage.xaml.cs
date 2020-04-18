using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using StockUp.Model;
using Xamarin.Forms;

namespace StockUp
{
    public partial class InventorySummaryPage : ContentPage
    {
        RestService _restService;
        public InventorySummaryPage()
        {
            InitializeComponent();
            PacketListView.ItemsSource = Constants.inventoryTickets;
            PacketListView.ItemTapped += Delete_Clicked;
        }

        protected async override void OnAppearing()
        {
            _restService = new RestService();
            HttpResponseMessage responseGames = await _restService.GetInventory();
			string contentGames = await responseGames.Content.ReadAsStringAsync();
			contentGames = Constants.TakeOutHeaderJSON(contentGames);
			Constants.inventoryTickets = JsonConvert.DeserializeObject<List<TicketData>>(contentGames);

            for (int i = 0; i<Constants.inventoryTickets.Count; i++)
            {
                Constants.inventoryTickets[i].Name = Constants.gamesAndNames[Constants.inventoryTickets[i].Game];
                Constants.inventoryTickets[i].Price = Constants.gamesAndPrices[Constants.inventoryTickets[i].Game];
                Constants.inventoryTickets[i].Id = Constants.CreateTicketId(Constants.inventoryTickets[i].Game, Constants.inventoryTickets[i].Pack, Constants.inventoryTickets[i].Nbr);
                Constants.inventoryTickets[i].Status = "Scanned";
            }
            PacketListView.ItemsSource = Constants.inventoryTickets;

        }

        async void Add_Clicked(System.Object sender, System.EventArgs e)
        {
            Constants.State = Constants.Inventory;
            await Navigation.PushModalAsync(new CustomScannerPage());
        }

        async void Delete_Clicked(System.Object sender, ItemTappedEventArgs e)
        {
            //var tappedUser = e.Item as UserData;
            //var answer = await DisplayAlert("Delete user " + tappedUser.First, "Would you like to delete this user?\n(You cannot revert this)", "Confirm", "Cancel");
            //if (answer)
            //{
            //    HttpResponseMessage[] responses = await _restService.DeleteUserData(Constants.StockUpEndpoint, tappedUser.Emp_id, tappedUser.Email);
            //    if (responses[0].IsSuccessStatusCode && responses[1].IsSuccessStatusCode)
            //    {
            //        await DisplayAlert("Success","User is now deleted", "OK");
            //        OnAppearing();
            //    }
            //    else
            //    {
            //        await DisplayAlert("Failure", "Something went wrong and the account was not deleted", "OK");
            //    }
            //}
        }

        void SearchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            PacketListView.BeginRefresh();
            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                PacketListView.ItemsSource = Constants.inventoryTickets;
            else
                PacketListView.ItemsSource = Constants.inventoryTickets.Where(i => i.Game == Convert.ToInt32(e.NewTextValue));
            PacketListView.EndRefresh();
        }
    }
}
