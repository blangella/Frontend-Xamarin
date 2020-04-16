using System;
using System.Collections.Generic;

using Xamarin.Forms;

using Microcharts.Forms;
using SkiaSharp;
using Microcharts;
using StockUp.Model;
using System.Net.Http;
using Newtonsoft.Json;
using StockUp.Model.Structs;
using System.Linq;
using System.Diagnostics;

namespace StockUp
{
    public partial class AnalyticsPage : ContentPage
    {
        RestService _restService;
        Dictionary<String, int> groupedTicketsData = new Dictionary<string, int>();
        List<Microcharts.Entry> entries = new List<Microcharts.Entry>();
        AnalyticsData[] gameCounts;
        AnalyticsData[] bestGames = new AnalyticsData[5];
        AnalyticsData[] worstGames = new AnalyticsData[5];
        int averageTicketsSoldPerGame;

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
            HttpResponseMessage response = await _restService.GetMonthlyCounts();
			string content = await response.Content.ReadAsStringAsync();
            content = Constants.TakeOutHeaderJSON(content);
			gameCounts = JsonConvert.DeserializeObject<AnalyticsData[]>(content);

            var sortedAsc = gameCounts.OrderBy(g => g.SumFinalTotal).ToList();
            gameCounts = sortedAsc.ToArray();

            for (int i = 0; i<gameCounts.Length; i++)
            {
                if (i < 5)
                {
                    worstGames[i] = gameCounts[i];
                }
            }

            var sortedDesc = gameCounts.OrderByDescending(g => g.SumFinalTotal).ToList();
            gameCounts = sortedDesc.ToArray();

            for (int i = 0; i<gameCounts.Length; i++)
            {
                if (i < 5)
                {
                    bestGames[i] = gameCounts[i];
                }
            }

            for (int i = 0; i<15; i++)
            {
                int value = gameCounts[i].SumFinalTotal;
                entries.Add(new Microcharts.Entry(value)
                {
                    Label = Constants.gamesAndNames[gameCounts[i].Game],
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
