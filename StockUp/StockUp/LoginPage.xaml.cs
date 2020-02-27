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
            if (employee.Text != null && employee.Text.ToLower().Equals("admin"))
            {
                Navigation.PushAsync(new AdminHomePage());
                //App.Current.MainPage = new AdminHomePage();
            }
            else if (employee.Text != null && !employee.Text.ToLower().Equals("admin"))
            {
                Navigation.PushAsync(new HomePage());
                //App.Current.MainPage = new HomePage();
            }
            else
            {
                DisplayAlert("Error", "Make sure to input the required credentials", "OK");
            }

        }

    }
}
