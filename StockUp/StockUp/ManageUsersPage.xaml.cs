using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using StockUp.Model;

namespace StockUp
{
    public partial class ManageUsersPage : ContentPage
    {
        public ManageUsersPage()
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
            await DisplayAlert("Checking checkboxes", addOrRemoveUser.IsToggled.ToString()+adminOrUser.IsToggled.ToString(), "OK");

            if (!addOrRemoveUser.IsToggled)
            {
                if (employeeID.Text != null && email.Text != null && firstName.Text != null && lastName.Text != null && password.Text != null && passwordConfirm.Text != null)
                {
                    if (!password.Text.Equals(passwordConfirm.Text))
                    {
                        await DisplayAlert("Cannot add user", "Passwords do not match.", "OK");
                    }
                    else
                    {
                        // TODO: Check for users that exist already before committing
                        int adminValue = 0;
                        if (!adminOrUser.IsToggled)
                        {
                            adminValue = 0;
                        }
                        else if(adminOrUser.IsToggled)
                        {
                            adminValue = 1;
                        }
                        User user = new User(employeeID.Text, firstName.Text, lastName.Text, email.Text, adminValue, password.Text);
                        await DisplayAlert("Success!", "Added user: "+user.Emp_id+" "+user.First+" "+user.Passsword, "OK");
                        await Navigation.PopModalAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Cannot add user", "Some of the fields are missing.", "OK");

                }
            } else if (addOrRemoveUser.IsToggled)
            {
                // TODO: Check if user exists
            }
            
        }
    }
}
