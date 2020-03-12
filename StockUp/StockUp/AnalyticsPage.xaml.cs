using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Microcharts.Forms;
using SkiaSharp;
using Microcharts;

namespace StockUp
{
    public partial class AnalyticsPage : ContentPage
    {
        List<Microcharts.Entry> entries = new List<Microcharts.Entry>
        {
            new Microcharts.Entry(200)
            {
                Label = "January",
                ValueLabel = "200",
                Color = SKColor.Parse("#266489")
            },
            new Microcharts.Entry(400)
            {
                Label = "February",
                ValueLabel = "400",
                Color = SKColor.Parse("#68B9C0")
            },
            new Microcharts.Entry(-100)
            {
                Label = "March",
                ValueLabel = "-100",
                Color = SKColor.Parse("#90D585")
            },
        };

        public AnalyticsPage()
        {
            InitializeComponent();
            chartView.Chart = new RadialGaugeChart() { Entries = entries };

        }

    }
}
