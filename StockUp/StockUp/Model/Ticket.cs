using System;
namespace StockUp.Model
{
    public class Ticket
    {
        public String Id { get; set; }

        public int Game { get; set; }
        public int Pack { get; set; }
        public int Nbr { get; set; }
        public DateTime Date { get; set; }

        public String Name { get; set; }
        public String Store { get; set; }
        public int Activated { get; set; } // tiny int 0 1
        public int LoadedPack { get; set; } // tiny int 0 1
        public int Price { get; set; }
        public int Start_Inv { get; set; } // tiny int 0 1 
        public int End_Inv { get; set; } // tiny int 0 1
        public String Emp_id { get; set; }

        public Boolean IsChecked { get; set; }
        public String Status { get; set; }
        public String IconSource { get; set; }

        public Ticket(int game, int packet, int ticket)
        {
            Game = game;
            Pack = packet;
            Nbr = ticket;
            Date = DateTime.Now;

            Id = Game.ToString() + Pack.ToString()+Nbr.ToString()+"-"+Date.ToShortDateString();

            Random rand = new Random();
            int r = rand.Next(2);  
            if (r == 0)
            {
                Status = "Checked";
                IsChecked = true;
                IconSource = "Status_Green.png";
            }
            else
            {
                Status = "Not Checked Yet";
                IsChecked = false;
                IconSource = "Status_Red.png";
            }

        }
    }

    
}

