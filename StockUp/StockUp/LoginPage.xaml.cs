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
            BindingContext = new LoginViewModel();
        }

        void Login_Clicked(System.Object sender, System.EventArgs e)
        {
            if (employee.Text.ToLower().Equals("admin"))
            {
                Navigation.PushAsync(new AdminHomePage());
            }
            else
            {
                Navigation.PushAsync(new HomePage());
            }

        }

    }
}
