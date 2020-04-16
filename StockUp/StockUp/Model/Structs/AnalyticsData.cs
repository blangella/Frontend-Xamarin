using System;
using Newtonsoft.Json;

namespace StockUp.Model.Structs
{
    public class AnalyticsData
    {
		[JsonProperty("SUM(FinalTotal)")]
		public int SumFinalTotal { get; set; }

		[JsonProperty("Game")]
		public int Game { get; set; }
    }
}
