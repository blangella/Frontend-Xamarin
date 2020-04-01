using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Windows.Input;
using StockUp.Model;
using ZXing.Net.Mobile.Forms;

namespace StockUp
{

    public partial class ScanSummaryPage : ContentPage
    {
        //ZXingScannerPage scanPage;
        RestService _restService;
        public ScanSummaryPage()
        {
            InitializeComponent();

            _restService = new RestService();
        }

        protected override async void OnAppearing()
        {
            TicketData[] ticketsData = await _restService.GetTicketsData(Constants.StockUpEndpoint, Constants.APIKey);
            PacketListView.ItemsSource = ticketsData;
        }
        

        async void Ticket_Tapped(object sender, EventArgs e)
        {
            if (((sender as Image).BindingContext is Ticket _selectedTicket))
            {
                //await DisplayAlert("View Profile", String.Format("{0} \n {1}", _selectedTicket.Id, _selectedTicket.Status), "Okay");
                //scanPage = new ZXingScannerPage ();
                //scanPage.OnScanResult += (result) => {
                //    scanPage.IsScanning = false;

                //    Device.BeginInvokeOnMainThread (() => {
                //        Navigation.PopModalAsync ();
                //        DisplayAlert("Scanned Barcode", result.Text, "OK");
                //    });
                //};

                //await Navigation.PushModalAsync (scanPage);

                await Navigation.PushModalAsync(new CustomScannerPage());
            }
        }

        async void Scan_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new CustomScannerPage());
        }

        async void Confirm_Clicked(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("Confirm All Scans", "Are you sure you want to confirm?", "OK");
        }
    }
}

