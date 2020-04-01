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
        Dictionary<String, int> groupedTicketsData = new Dictionary<string, int>();
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

            
        }

        protected override async void OnAppearing()
        {
            TicketData[] ticketsData = await _restService.GetTicketsData(Constants.StockUpEndpoint, Constants.APIKey);
            foreach (TicketData t in ticketsData)
            {
                if (groupedTicketsData.ContainsKey(t.Name))
                {
                    groupedTicketsData[t.Name]++;
                }
                else
                {
                    groupedTicketsData.Add(t.Name, 1);
                }
            }

            foreach (KeyValuePair<string, int> group in groupedTicketsData)
            {
                int value = group.Value;
                entries.Add(new Microcharts.Entry(value)
                {
                    Label = group.Key,
                    ValueLabel = value.ToString(),
                    Color = SKColor.Parse(Constants.GetRandomColor())
                });
            }
            donutChart.Chart = new DonutChart() { Entries = entries };
            //barChart.Chart = new BarChart() { Entries = entries };
            //pointChart.Chart = new PointChart() { Entries = entries };
            //lineChart.Chart = new LineChart() { Entries = entries };
            //radialGaugeChart.Chart = new RadialGaugeChart() { Entries = entries };
            //radarChart.Chart = new RadarChart() { Entries = entries };
        }
    }
}
