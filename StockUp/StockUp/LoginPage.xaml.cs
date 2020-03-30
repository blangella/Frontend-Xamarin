using Xamarin.Forms;
using MySql.Data.MySqlClient;
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

        public void Login_Clicked(System.Object sender, System.EventArgs e)
        {
            if (employee.Text != null && password.Text != null)
            {
                MySqlConnection mySqlConnection = new MySqlConnection()
                {
                    ConnectionString = "Server=db431.cjeog1zo7yyf.us-east-2.rds.amazonaws.com;" +
                "Port=3306;" +
                "Database=SIS;" +
                "UID=master431;" +
                "Pwd=masterhot1;"
                };

                //MySqlParameter emp_id = new MySqlParameter
                //{
                //    ParameterName = "@username",
                //    Value = employee.Text
                //};

                //MySqlParameter pass = new MySqlParameter
                //{
                //    ParameterName = "@password",
                //    Value = password.Text
                //};

                //MySqlCommand cmd = new MySqlCommand("SELECT IsAdmin FROM tblUsers WHERE Emp_id=@username AND Password=@password;", mySqlConnection);
                //cmd.Parameters.Add(pass);
                //cmd.Parameters.Add(emp_id);

                mySqlConnection.Open();
                //object un = cmd.ExecuteScalar();
                mySqlConnection.Close();

                //if (un.ToString().Equals("1"))
                //{
                //    NavigationPage page = new NavigationPage(new AdminHomePage());
                //    App.Current.MainPage = page;
                //    Navigation.PopToRootAsync();
                //}
                //else if (un.ToString().Equals("0"))
                //{
                //    NavigationPage page = new NavigationPage(new HomePage());
                //    App.Current.MainPage = page;
                //    Navigation.PopToRootAsync();
                //}
                //else
                //{
                //    DisplayAlert("Error", "Invalid User", "OK");
                //}
            }
            else
            {
                DisplayAlert("Error", "Make sure to input the required credentials", "OK");
            }

        }
    }
}