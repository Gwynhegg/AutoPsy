using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AutoPsy.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            var databaseConnector = Database.DatabaseConnector.GetDatabaseConnector();
            var usersExisted = databaseConnector.IsTableExisted("Users");
            if (usersExisted)
            {
                NavigateToLoginPage();
                return;
            }
            else SetPrivacyPoliceView();
        }

        private async void NavigateToLoginPage() => await Navigation.PushModalAsync(new Pages.LogInPage());
        private async void AcceptPolicy_Clicked(object sender, EventArgs e) => await Navigation.PushModalAsync(new Pages.RegisterPage());
        private void SetPrivacyPoliceView()
        {
            LoadingView.IsVisible = false;
            PolicyView.IsVisible = true;
        }

    }
}