using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Microcharts.Forms;
using SkiaSharp;
using Microcharts;
using StockUp.Model;

namespace StockUp
{
    public partial class AnalyticsPage : ContentPage
    {
        RestService _restService;
        List<TicketData[]> groupedTicketsData = new List<TicketData[]>();
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
            _restService = new RestService();

            for (int i = 0; i<groupedTicketsData.Count; i++)
            {
                int value = groupedTicketsData[i].Length;
                entries.Add(new Microcharts.Entry(value)
                {
                    Label = groupedTicketsData[i][0].Name,
                    ValueLabel = value.ToString(),
                    Color = SKColor.Parse(Constants.GetRandomColor())
                });
            }

            //barChart.Chart = new BarChart() { Entries = entries };
            //pointChart.Chart = new PointChart() { Entries = entries };
            //lineChart.Chart = new LineChart() { Entries = entries };
            donutChart.Chart = new DonutChart() { Entries = entries };
            //radialGaugeChart.Chart = new RadialGaugeChart() { Entries = entries };
            //radarChart.Chart = new RadarChart() { Entries = entries };
        }

        protected override async void OnAppearing()
        {
            TicketData[] ticketsData = await _restService.GetTicketsData(Constants.StockUpEndpoint, Constants.APIKey);
            //groupedTicketsData.

            /*
             *  TODO:
             *  Find a way to filter through ticketsData and create groupedTicketsData by game number
             */
        }
    }
}
