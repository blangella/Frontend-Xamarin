using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using StockUp.Model;

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
        }
        protected override async void OnAppearing()
        {
            UserData[] usersData = await _restService.GetUsersData(Constants.StockUpEndpoint, Constants.APIKey);
            UserListView.ItemsSource = usersData;
        }
        async void Add_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new AddUserPage());
        }
    }
}
