using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StockUp
{
    public partial class AdminHomePage : ContentPage
    {
        public AdminHomePage()
        {
            InitializeComponent();
        }

        void Logout_Clicked(System.Object sender, System.EventArgs e)
        {
            NavigationPage page = new NavigationPage(new LoginPage());
            App.Current.MainPage = page;
            Navigation.PopToRootAsync();
        }

        async void ManageUsers_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new ManageUsersPage());
        }

        async void Analytics_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AnalyticsPage());
        }
    }
}
