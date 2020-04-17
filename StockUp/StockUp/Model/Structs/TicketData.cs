using System;
using Newtonsoft.Json;

namespace StockUp.Model
{
public class TicketData
	{
		[JsonProperty("Game")]
		public int Game { get; set;}

		[JsonProperty("Pack")]
		public int Pack { get; set;}
		//
		[JsonProperty("Nbr")]
		public int Nbr { get; set; }

		[JsonProperty("Name")]
		public String Name { get; set; }

		[JsonProperty("Store")]
		public String Store { get; set; }

		[JsonProperty("Date")]
		public DateTime Date { get; set; }

		[JsonProperty("Activated")]
		public int Activated { get; set; } // tiny int 0 1

		[JsonProperty("LoadedPack")]
		public int LoadedPack { get; set; } // tiny int 0 1

		[JsonProperty("Price")]
		public String Price { get; set; }

		[JsonProperty("Start_Inv")]
		public int Start_Inv { get; set; } // tiny int 0 1

		[JsonProperty("End_Inv")]
		public int End_Inv { get; set; } // tiny int 0 1

		[JsonProperty("Emp_id")]
		public String Emp_id { get; set; }

		// for UI
		public String Id { get; set; }
		public String IconSource = "Status_Green.png";
		public String Status { get; set; }
        public bool isScanned { get; set; }

	}

	public class GameNamePriceData
	{
		[JsonProperty("Game")]
		public int Game { get; set; }

		[JsonProperty("Name")]
		public string Name { get; set; }

		[JsonProperty("Price")]
		public string Price { get; set; }
	}
}
