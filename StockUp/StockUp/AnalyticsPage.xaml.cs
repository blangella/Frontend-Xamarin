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
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _restService = new RestService();
            HttpResponseMessage response = await _restService.GetMonthlyCounts();
			string content = await response.Content.ReadAsStringAsync();
            content = Constants.TakeOutHeaderJSON(content);
			gameCounts = JsonConvert.DeserializeObject<AnalyticsData[]>(content);

            var sortedAsc = gameCounts.OrderBy(g => g.SumFinalTotal).ToList();
            gameCounts = sortedAsc.ToArray();

            for (int i = 0; i<gameCounts.Length; i++)
            {
                gameCounts[i].Name = Constants.gamesAndNames[gameCounts[i].Game];
            }

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

            int gamesSum = 0;
            int revenueSum = 0;
            for (int i = 0; i<gameCounts.Length; i++)
            {
                gamesSum += gameCounts[i].SumFinalTotal;
                var price = Constants.gamesAndPrices[gameCounts[i].Game];
                Debug.Write("PRICE: " + price);
                
                revenueSum += Convert.ToInt32(price);
            }

            var avg = (gamesSum / gameCounts.Length);
            averageTickets.Text = avg.ToString();
            totalRevenue.Text = "$"+revenueSum.ToString();
            donutChart.Chart = new DonutChart() { Entries = entries };

            BestListView.ItemsSource = bestGames;
            WorstListView.ItemsSource = worstGames;
            //barChart.Chart = new BarChart() { Entries = entries };
            //pointChart.Chart = new PointChart() { Entries = entries };
            //lineChart.Chart = new LineChart() { Entries = entries };
            //radialGaugeChart.Chart = new RadialGaugeChart() { Entries = entries };
            //radarChart.Chart = new RadarChart() { Entries = entries };
        }
    }
}
