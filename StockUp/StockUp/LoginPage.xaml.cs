using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StockUp
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void Login_Clicked(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new HomePage());
        }

    }
}
