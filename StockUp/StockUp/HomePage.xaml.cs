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
    }
}
