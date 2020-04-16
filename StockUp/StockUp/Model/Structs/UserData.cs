using System;
using Newtonsoft.Json;

namespace StockUp.Model
{
	public class UserData
	{
		[JsonProperty("IsAdmin")]
		public int IsAdmin { get; set; } // tiny int 0 1

		[JsonProperty("Emp_id")]
		public String Emp_id { get; set; }

		[JsonProperty("First")]
		public String First { get; set; }

		[JsonProperty("Last")]
		public String Last { get; set; }

		[JsonProperty("Email")]
		public String Email { get; set; }

		[JsonProperty("Password")]
		public String Passsword { get; set; }
	}
}
