using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StockUp
{
    public partial class ScanSummaryPage : ContentPage
    {
        public ScanSummaryPage()
        {
            InitializeComponent();
            int cap = 30;
            Ticket[] tickets = new Ticket[cap];
            for (int i = 0; i<cap; i++)
            {
                tickets[i] = new Ticket(i+300,i+200, i);
            }

            PacketListView.ItemsSource = tickets;
        }
    }
}

class Ticket {
    public string Id
    {
        get;
        set;
    }

    public string Status
    {
        get;
        set;
    }

    public Boolean IsChecked
    {
        get;
        set;
    }

    public int GameNumber
    {
        get;
        set;
    }

    public int PacketNumber
    {
        get;
        set;
    }

    public int TicketNumber
    {
        get;
        set;
    }

    public Ticket(int game, int packet, int ticket)
    {
        GameNumber = game;
        PacketNumber = packet;
        TicketNumber = ticket;

        Id = GameNumber.ToString() +"-"+ PacketNumber.ToString();

        Random rand = new Random();
        int r = rand.Next(2);  
        if (r == 0)
        {
            Status = "Checked";
            IsChecked = true;
        }
        else
        {
            Status = "Not Checked Yet";
            IsChecked = false;
        }

    }
}

