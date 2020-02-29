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
                NavigationPage page = new NavigationPage(new AdminHomePage());
                App.Current.MainPage = page;
                Navigation.PopToRootAsync();
            }
            else if (employee.Text != null && !employee.Text.ToLower().Equals("admin"))
            {
                NavigationPage page = new NavigationPage(new HomePage());
                App.Current.MainPage = page;
                Navigation.PopToRootAsync();
            }
            else
            {
                DisplayAlert("Error", "Make sure to input the required credentials", "OK");
            }

        }

    }
}
