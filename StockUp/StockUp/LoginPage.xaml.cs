using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using StockUp.Model;
using Xamarin.Forms;

namespace StockUp
{
	public partial class LoginPage : ContentPage
	{
		RestService _restService;
		public LoginPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			BindingContext = new LoginViewModel();
		}

		async void Login_Clicked(System.Object sender, System.EventArgs e)
		{
			//if (employee.Text != null && employee.Text.ToLower().Equals("admin"))
			//{
			//    NavigationPage page = new NavigationPage(new AdminHomePage());
			//    App.Current.MainPage = page;
			//    Navigation.PopToRootAsync();
			//}
			//else if (employee.Text != null && !employee.Text.ToLower().Equals("admin"))
			//{
			//    NavigationPage page = new NavigationPage(new HomePage());
			//    App.Current.MainPage = page;
			//    Navigation.PopToRootAsync();
			//}
			if (!string.IsNullOrWhiteSpace(employee.Text) && !string.IsNullOrWhiteSpace(password.Text))
			{
				_restService = new RestService();
				var url = Constants.StockUpEndpoint + "Users/login";
				//
				var response = await _restService.PostUserLogin(url, employee.Text, password.Text);

                var responseCode = response.IsSuccessStatusCode;
                if (responseCode)
                {
					LoginData loginData;
                    var json = await response.Content.ReadAsStringAsync();
                    loginData = JsonConvert.DeserializeObject<LoginData>(json);
                    Constants.APIKey = loginData.Id.ToString();
					//await DisplayAlert("Success", loginData.Id, "OK");
					UserData userData;
					var userResponse = await _restService.GetUserData(Constants.StockUpEndpoint, employee.Text);
                    var userJson = await userResponse.Content.ReadAsStringAsync();
                    userData = JsonConvert.DeserializeObject<UserData>(userJson);
                    if (userData.IsAdmin == 1)
                    {
                        NavigationPage page = new NavigationPage(new AdminHomePage());
                        App.Current.MainPage = page;
                        await Navigation.PopToRootAsync();
                    }
                    else if (userData.IsAdmin == 0)
                    {
                        NavigationPage page = new NavigationPage(new HomePage());
                        App.Current.MainPage = page;
                        await Navigation.PopToRootAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Error", "Username or Password is incorrect", "OK");
                }
				
			}
			else
			{
				await DisplayAlert("Error", "Make sure to input the required credentials", "OK");
			}

		}

		async void Create_Clicked(System.Object sender, System.EventArgs e)
		{
            await Navigation.PushModalAsync(new SignupPage());
		}

	}
}
