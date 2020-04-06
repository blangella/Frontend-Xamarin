using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using StockUp.Model;
using System.Net.Http;
using System.Linq;

namespace StockUp
{

    /*
     * TODO: Maybe make admin go to "manage users" page where...
     *      - there is a list of users
     *      - click a user -> pop up if they want to delete user
     *      - add button top right -> modal form sheet of adding a user
     */
    public partial class ManageUsersPage : ContentPage
    {
        RestService _restService;
        public ManageUsersPage()
        {
            InitializeComponent();

            _restService = new RestService();
            UserListView.ItemTapped += Delete_Clicked;
        }
        protected override async void OnAppearing()
        {
            UserData[] usersData = await _restService.GetUsersData(Constants.StockUpEndpoint, Constants.APIKey);
            UserListView.ItemsSource = usersData.Where(u => u.Emp_id != Constants.UserData.Emp_id).ToArray();
        }
        async void Add_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new AddUserPage());
        }
        async void Delete_Clicked(System.Object sender, ItemTappedEventArgs e)
        {
            var tappedUser = e.Item as UserData;
            var answer = await DisplayAlert("Delete user " + tappedUser.First, "Would you like to delete this user?\n(You cannot revert this)", "Confirm", "Cancel");
            if (answer)
            {
                HttpResponseMessage[] responses = await _restService.DeleteUserData(Constants.StockUpEndpoint, tappedUser.Emp_id, tappedUser.Email);
                if (responses[0].IsSuccessStatusCode && responses[1].IsSuccessStatusCode)
                {
                    await DisplayAlert("Success","User is now deleted", "OK");
                    OnAppearing();
                }
                else
                {
                    await DisplayAlert("Failure", "Something went wrong and the account was not deleted", "OK");
                }
            }
        }
    }
}
