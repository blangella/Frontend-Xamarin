using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace StockUp
{
    public partial class HomePage : ContentPage
    {
        ZXingScannerPage scanPage;
        public HomePage()
        {
            InitializeComponent();
            //NavigationPage.SetHasBackButton(this, false);
        }

        async void Start_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ScanSummaryPage());
            //scanPage = new ZXingScannerPage ();
            //scanPage.OnScanResult += (result) => {
            //    scanPage.IsScanning = false;

            //    Device.BeginInvokeOnMainThread (() => {
            //        Navigation.PopModalAsync ();
            //        DisplayAlert("Scanned Barcode", result.Text, "OK");
            //    });
            //};

            //await Navigation.PushModalAsync (scanPage);
        }

        async void Activate_Clicked(System.Object sender, System.EventArgs e)
        {
            scanPage = new ZXingScannerPage ();
            scanPage.OnScanResult += (result) => {
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread (() => {
                    Navigation.PopModalAsync ();
                    DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            await Navigation.PushModalAsync (scanPage);
        }

        async void End_Clicked(System.Object sender, System.EventArgs e)
        {
            scanPage = new ZXingScannerPage ();
            scanPage.OnScanResult += (result) => {
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread (() => {
                    Navigation.PopModalAsync ();
                    DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };

            await Navigation.PushModalAsync (scanPage);
        }

        void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
            NavigationPage page = new NavigationPage(new LoginPage());
            App.Current.MainPage = page;
            Navigation.PopToRootAsync();
        }
    }
}
