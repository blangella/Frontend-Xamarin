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
            if (employee.Text != null)
            {
                string result = DBConncection.Login(employee.Text, password.Text);
                NavigationPage page;
                switch (result)
                {
                    case null:
                        DisplayAlert("Error", "Invalid User", "OK");
                        break;
                    case "1":
                        page = new NavigationPage(new AdminHomePage());
                        App.Current.MainPage = page;
                        Navigation.PopToRootAsync();
                        break;
                    case "0":
                        page = new NavigationPage(new HomePage());
                        App.Current.MainPage = page;
                        Navigation.PopToRootAsync();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                DisplayAlert("Error", "Make sure to input the required credentials", "OK");
            }

        }
    }
}
