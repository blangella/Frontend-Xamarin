using System;
using System.Collections.Generic;

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
                var action = await DisplayActionSheet("Scanned Barcode\n" + result.Text, "Redo", null, actionButtons);
                switch (action)
                {
                    case "Redo":
                        Zxing.IsAnalyzing = true;
                        break;
                    case "Confirm":
                        ScannedTickets.Add(result.Text); //
                        Zxing.IsAnalyzing = true;
                        break;
                    case "Done":
                        ScannedTickets.Add(result.Text);
                        String tickets = "";
                        for (int i = 0; i<ScannedTickets.Count; i++)
                        {
                                tickets += ", "+ScannedTickets[i];
                        }
                        await DisplayAlert("Going back to summary page with new scans", tickets, "OK");
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
