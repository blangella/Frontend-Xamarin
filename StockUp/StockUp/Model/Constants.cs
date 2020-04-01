using System;
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

        public static String GetRandomColor()
        {
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
            return color;
        }
    }


}
