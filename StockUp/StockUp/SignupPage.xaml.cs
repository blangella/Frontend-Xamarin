using System;
using System.Collections.Generic;
using StockUp.Model;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace StockUp
{
    public partial class SignupPage : ContentPage
    {
		RestService _restService;

        public SignupPage()
        {
            InitializeComponent();
            On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
        }

        async void Cancel_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void Confirm_Clicked(System.Object sender, System.EventArgs e)
        {
            if (email.Text != null && firstName.Text != null && lastName.Text != null && password.Text != null && passwordConfirm.Text != null)
            {
                if (!password.Text.Equals(passwordConfirm.Text))
                {
                    await DisplayAlert("Cannot add user", "Passwords do not match.", "OK");
                }
                else
                {
                    //User user = new User(employeeID.Text, firstName.Text, lastName.Text, email.Text, adminValue, password.Text);
                    _restService = new RestService();
                    var response = await _restService.PostNewAdmin(Constants.StockUpEndpoint, email.Text, firstName.Text, lastName.Text, password.Text);

                    var authResponseCode = response[0].IsSuccessStatusCode;
                    var tblResponseCode = response[1].IsSuccessStatusCode;

                    if (authResponseCode && tblResponseCode)
                    {
                        await DisplayAlert("Success!", "Added user", "OK");
                        var authJson = await response[0].Content.ReadAsStringAsync();
                        await DisplayAlert("Auth", authJson.ToString(), "OK");
                        var tblJson = await response[1].Content.ReadAsStringAsync();
                        await DisplayAlert("Tbl", tblJson.ToString(), "OK");
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Failed", "Could not add user", "OK");
                    }
                    
                }
            }
            else
            {
                await DisplayAlert("Cannot add user", "Some of the fields are missing.", "OK");

            }            
        }
    }
}
