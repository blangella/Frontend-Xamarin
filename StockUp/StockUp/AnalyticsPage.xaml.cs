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
        List<Microcharts.Entry> entries = new List<Microcharts.Entry>();
        //{
        //    new Microcharts.Entry(200)
        //    {
        //        Label = "January",
        //        ValueLabel = "200",
        //        Color = SKColor.Parse(AnalyticsPage.GetRandomColor())
        //    },
        //    new Microcharts.Entry(400)
        //    {
        //        Label = "February",
        //        ValueLabel = "400",
        //        Color = SKColor.Parse(AnalyticsPage.GetRandomColor())
        //    },
        //    new Microcharts.Entry(-100)
        //    {
        //        Label = "March",
        //        ValueLabel = "100",
        //        Color = SKColor.Parse(AnalyticsPage.GetRandomColor())
        //    },
        //};

        public AnalyticsPage()
        {
            InitializeComponent();

            for (int i = 0; i<5; i++)
            {
                Random rand = new Random();
                int value = rand.Next(6);
                value *= 100;
                entries.Add(new Microcharts.Entry(value)
                {
                    Label = "label" + i,
                    ValueLabel = value.ToString(),
                    Color = SKColor.Parse(AnalyticsPage.GetRandomColor())
                });
            }

            barChart.Chart = new BarChart() { Entries = entries };
            pointChart.Chart = new PointChart() { Entries = entries };
            lineChart.Chart = new LineChart() { Entries = entries };
            donutChart.Chart = new DonutChart() { Entries = entries };
            radialGaugeChart.Chart = new RadialGaugeChart() { Entries = entries };
            radarChart.Chart = new RadarChart() { Entries = entries };
        }

        public static String GetRandomColor()
        {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
            return color;
        }
    }
}
