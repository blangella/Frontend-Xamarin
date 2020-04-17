using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Newtonsoft.Json;

namespace StockUp.Model
{
	public static class Constants
	{
		public const string StockUpEndpoint = "http://192.168.1.160:3000/api/";
		public static string APIKey = "INSERT_API_KEY_HERE";
		public static UserData UserData;
		public const string Start = "start";
		public const string End = "end";
		public const string Activate = "activate";
		public const string Inventory = "inventory";
		public static Dictionary<int, string> gamesAndNames = new Dictionary<int, string>();
		public static Dictionary<int, string> gamesAndPrices = new Dictionary<int, string>();
		public static TicketData[] startTickets;


		public static String GetRandomColor()
		{
			var random = new Random();
			var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
			return color;
		}

		public static void InitializeAllGames(string content)
		{
			GameNamePriceData[] games = JsonConvert.DeserializeObject<GameNamePriceData[]>(content);
			Debug.Write("In Initialize");
			for (int i = 0; i < games.Length; i++)
			{
                if (!gamesAndNames.ContainsKey(games[i].Game))
                {
				    Debug.Write("Adding");
				    gamesAndNames.Add(games[i].Game, games[i].Name);
				    gamesAndPrices.Add(games[i].Game, games[i].Price);
                }
			}
		}

        public static String CreateTicketId(int Game, int Pack, int Nbr)
        {
            return Game.ToString() + " - " + Pack.ToString() + " - " + Nbr.ToString();
        }

		public static String TakeOutHeaderJSON(String JSON)
		{
			JSON = JSON.Substring(0, JSON.Length - 1 - 2);
			Debug.Write("\nJSON: "+JSON);

			int i = JSON.Length-1;
			while (JSON[i] != ']')
			{
				i--;
			}
			JSON = JSON.Substring(0, i+1);
			Debug.Write("\nJSON: "+JSON);

			JSON = JSON.Substring(1, JSON.Length-1);
			Debug.Write("\nJSON: "+JSON);

			return JSON;
		}

		public static int GetGameNum(String barcode)
		{
			barcode = barcode.Substring(0, 5);
			int i = 0;
			while (i < barcode.Length)
			{
				if (barcode[i] == '0')
				{
					break;
				}
			}
			barcode = barcode.Substring(i + 1);
			Debug.Write("GAME: " + barcode);
			return Int16.Parse(barcode);
		}

		public static int GetPackNum(String barcode)
		{
			barcode = barcode.Substring(5, 6);
			int i = 0;
			while (i < barcode.Length)
			{
				if (barcode[i] == '0')
				{
					break;
				}
			}
			barcode = barcode.Substring(i + 1);
			Debug.Write("PACK: " + barcode);
			return Int32.Parse(barcode);
		}

		public static int GetNbrNum(String barcode)
		{
			barcode = barcode.Substring(11, 3);
			int i = 0;
			while (i < barcode.Length)
			{
				if (barcode[i] == '0')
				{
					break;
				}
			}
			barcode = barcode.Substring(i + 1);
			Debug.Write("NBR: " + barcode);
			return Int32.Parse(barcode);
		}
	}


}
